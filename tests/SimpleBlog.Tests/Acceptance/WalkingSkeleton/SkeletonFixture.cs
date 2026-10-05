using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;

namespace SimpleBlog.Tests.Acceptance.WalkingSkeleton;

// The app has no DbContext yet, so there is no database to isolate (no SimpleBlog_Test needed).
// The app runs on a real Kestrel server so browsers can reach it; HTTP tests use the same server.
public sealed class SkeletonFixture : IAsyncLifetime, IDisposable
{
    public const string BlogName = "Trình's Dev Notes";

    private readonly Dictionary<string, IBrowser> _browsers = [];
    private WebApplicationFactory<Program>? _factory;
    private IPlaywright? _playwright;

    public Uri BaseAddress { get; private set; } = null!;

    public HttpClient Http { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _factory.UseKestrel(0);
        _factory.StartServer();
        BaseAddress = _factory.ClientOptions.BaseAddress;

        Http = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }) { BaseAddress = BaseAddress };

        _playwright = await Playwright.CreateAsync();
        _browsers["chromium"] = await _playwright.Chromium.LaunchAsync();
        _browsers["firefox"] = await _playwright.Firefox.LaunchAsync();
        _browsers["webkit"] = await _playwright.Webkit.LaunchAsync();
    }

    public async Task DisposeAsync()
    {
        foreach (var browser in _browsers.Values)
        {
            await browser.CloseAsync();
        }

        _playwright?.Dispose();
    }

    public void Dispose()
    {
        Http?.Dispose();
        _factory?.Dispose();
    }

    public async Task<PageSession> OpenAsync(
        string browser,
        string path,
        int width = 1280,
        int height = 800,
        bool javaScript = true,
        bool bypassCsp = false,
        bool navigate = true)
    {
        var context = await _browsers[browser].NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = width, Height = height },
            JavaScriptEnabled = javaScript,
            BypassCSP = bypassCsp,
        });
        var page = await context.NewPageAsync();
        var session = new PageSession(context, page, new Uri(BaseAddress, path));
        if (navigate)
        {
            await session.GotoAsync();
        }

        return session;
    }

    public async Task<(HttpResponseMessage Response, string Html)> GetAsync(string path)
    {
        var response = await Http.GetAsync(path);
        var bytes = await response.Content.ReadAsByteArrayAsync();
        return (response, Encoding.UTF8.GetString(bytes).Normalize(NormalizationForm.FormC));
    }
}

public sealed class PageSession : IAsyncDisposable
{
    private readonly IBrowserContext _context;
    private readonly Uri _url;

    public PageSession(IBrowserContext context, IPage page, Uri url)
    {
        _context = context;
        Page = page;
        _url = url;
        page.Request += (_, request) => Requests.Add(request);
    }

    public IPage Page { get; }

    public List<IRequest> Requests { get; } = [];

    public async Task GotoAsync()
    {
        var response = await Page.GotoAsync(_url.ToString(), new PageGotoOptions { WaitUntil = WaitUntilState.Load });
        Assert.Equal(200, response?.Status);
    }

    public async Task<Rect> RectAsync(string selector)
    {
        var a = await Page.EvaluateAsync<double[]>(
            "s => { const r = document.querySelector(s).getBoundingClientRect(); return [r.left, r.top, r.right, r.bottom]; }",
            selector);
        return new Rect(a[0], a[1], a[2], a[3]);
    }

    public Task<string> StyleAsync(string selector, string property)
        => Page.EvaluateAsync<string>(
            "([s, p]) => getComputedStyle(document.querySelector(s))[p]",
            new[] { selector, property });

    public async ValueTask DisposeAsync() => await _context.CloseAsync();
}

public readonly record struct Rect(double Left, double Top, double Right, double Bottom)
{
    public double Width => Right - Left;

    public double Height => Bottom - Top;
}

[CollectionDefinition(Name)]
public sealed class SkeletonTestGroup : ICollectionFixture<SkeletonFixture>
{
    public const string Name = "WalkingSkeleton";
}

public static class SkeletonData
{
    public static readonly string[] Browsers = ["chromium", "firefox", "webkit"];
    public static readonly int[] Widths = [320, 360, 768, 1280, 1920];
    public static readonly string[] Pages = ["/", "/Home/About"];

    public static TheoryData<string, int, string> All()
        => Matrix(Browsers, Widths, Pages);

    public static TheoryData<string, int, string> Matrix(string[] browsers, int[] widths, string[] pages)
    {
        var data = new TheoryData<string, int, string>();
        foreach (var b in browsers)
        {
            foreach (var w in widths)
            {
                foreach (var p in pages)
                {
                    data.Add(b, w, p);
                }
            }
        }

        return data;
    }

    public static TheoryData<string, string> BrowserPage()
    {
        var data = new TheoryData<string, string>();
        foreach (var b in Browsers)
        {
            foreach (var p in Pages)
            {
                data.Add(b, p);
            }
        }

        return data;
    }

    public static TheoryData<string> PageOnly()
    {
        var data = new TheoryData<string>();
        foreach (var p in Pages)
        {
            data.Add(p);
        }

        return data;
    }

    public static string Decode(string html) => WebUtility.HtmlDecode(html).Normalize(NormalizationForm.FormC);

    public static string? Title(string html)
    {
        var m = Regex.Match(html, @"<title>(.*?)</title>", RegexOptions.Singleline);
        return m.Success ? Decode(m.Groups[1].Value) : null;
    }

    public static string? MetaDescription(string html)
    {
        var m = Regex.Match(html, @"<meta\s+name=""description""\s+content=""([^""]*)""");
        return m.Success ? Decode(m.Groups[1].Value) : null;
    }

    public static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "SimpleBlog.slnx")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("SimpleBlog.slnx not found");
    }
}
