using System.Text.Json;
using System.Text.RegularExpressions;
using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;

namespace SimpleBlog.Tests.Acceptance.WalkingSkeleton;

[Collection(SkeletonTestGroup.Name)]
[Trait("Category", "Browser")]
public class InteractionBrowserTests(SkeletonFixture fx)
{
    private const string MenuLabel = "Giới thiệu";

    public static TheoryData<string, string> BrowserPage() => SkeletonData.BrowserPage();

    // Playwright WebKit on Windows and macOS does not Tab to links (only form controls), like Safari by default.
    // Keyboard tests for WebKit therefore run only where it does (Linux, i.e. CI).
    public static TheoryData<string, string> KeyboardBrowserPage()
    {
        var data = new TheoryData<string, string>();
        foreach (var b in SkeletonData.Browsers)
        {
            if (b == "webkit" && (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS()))
            {
                continue;
            }

            foreach (var p in SkeletonData.Pages)
            {
                data.Add(b, p);
            }
        }

        return data;
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC4_BamTenBlog_VeTrangChu(string browser)
    {
        await using var s = await fx.OpenAsync(browser, "/Home/About");
        var label = (await s.Page.InnerTextAsync(".site-title")).Trim();
        Assert.Equal(SkeletonFixture.BlogName, label);
        await s.Page.ClickAsync(".site-title");
        await s.Page.WaitForURLAsync(new Uri(fx.BaseAddress, "/").ToString());
        Assert.Equal("/", new Uri(s.Page.Url).AbsolutePath);
        Assert.Equal("Bài viết", (await s.Page.InnerTextAsync("main h1")).Trim());
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC5_BamMucGioiThieu_MoTrangGioiThieu(string browser)
    {
        await using var s = await fx.OpenAsync(browser, "/");
        await s.Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = MenuLabel, Exact = true }).ClickAsync();
        await s.Page.WaitForURLAsync(new Uri(fx.BaseAddress, "/Home/About").ToString());
        Assert.Equal("Lê Khánh Trình", (await s.Page.InnerTextAsync("main h1")).Trim());
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC7_MucMenuTrangHienTai_DanhDauVaGachChan(string browser)
    {
        await using (var about = await fx.OpenAsync(browser, "/Home/About"))
        {
            Assert.Equal("page", await about.Page.GetAttributeAsync("nav a", "aria-current"));
            var line = await about.StyleAsync("nav a", "textDecorationLine");
            Assert.Contains("underline", line);
            Assert.Equal("rgb(10, 88, 202)", await about.StyleAsync("nav a", "textDecorationColor"));
        }

        await using var home = await fx.OpenAsync(browser, "/");
        Assert.Null(await home.Page.GetAttributeAsync("nav a", "aria-current"));
    }

    [Theory]
    [MemberData(nameof(KeyboardBrowserPage))]
    public async Task AC11_TabDauTien_LienKetBoQuaNhanFocusVaHienRa(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        await s.Page.Keyboard.PressAsync("Tab");
        var info = await s.Page.EvaluateAsync<JsonElement>(
            """
            () => { const e = document.activeElement; const r = e.getBoundingClientRect();
              return { cls: e.className, text: e.textContent.trim(), top: r.top, bottom: r.bottom, left: r.left, right: r.right,
                       vw: innerWidth, vh: innerHeight, vis: getComputedStyle(e).visibility }; }
            """);
        Assert.Equal("skip-link", info.GetProperty("cls").GetString());
        Assert.Equal("Bỏ qua tới nội dung chính", info.GetProperty("text").GetString());
        Assert.True(info.GetProperty("top").GetDouble() >= 0 && info.GetProperty("bottom").GetDouble() <= info.GetProperty("vh").GetDouble(), "lien ket bo qua khong hien trong khung nhin");
        Assert.True(info.GetProperty("left").GetDouble() >= 0 && info.GetProperty("right").GetDouble() <= info.GetProperty("vw").GetDouble());
        Assert.Equal("visible", info.GetProperty("vis").GetString());
    }

    [Theory]
    [MemberData(nameof(BrowserPage))]
    public async Task AC12_LienKetBoQuaAnKhiChuaCoFocus(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        var r = await s.RectAsync(".skip-link");
        Assert.True(r.Bottom <= 0 || r.Top >= 800 || r.Right <= 0, $"lien ket bo qua dang nhin thay: {r}");
    }

    [Theory]
    [MemberData(nameof(KeyboardBrowserPage))]
    public async Task AC13_EnterTrenLienKetBoQua_FocusVaoMain(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        await s.Page.Keyboard.PressAsync("Tab");
        await s.Page.Keyboard.PressAsync("Enter");
        await s.Page.WaitForFunctionAsync("() => document.activeElement && document.activeElement.id === 'main'", null, new PageWaitForFunctionOptions { Timeout = 3000 });
        Assert.Equal("main", await s.Page.EvaluateAsync<string>("() => document.activeElement.tagName.toLowerCase()"));
    }

    [Theory]
    [MemberData(nameof(KeyboardBrowserPage))]
    public async Task AC14_FocusLuonNhinThay(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        var visited = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            await s.Page.Keyboard.PressAsync("Tab");
            var info = await s.Page.EvaluateAsync<JsonElement>(
                """
                () => { const e = document.activeElement; const cs = getComputedStyle(e); const r = e.getBoundingClientRect();
                  return { body: e === document.body || e === document.documentElement, id: e.className || e.tagName,
                    w: parseFloat(cs.outlineWidth), style: cs.outlineStyle,
                    top: r.top, bottom: r.bottom, left: r.left, right: r.right, vw: innerWidth, vh: innerHeight }; }
                """);
            if (info.GetProperty("body").GetBoolean())
            {
                break;
            }

            var id = info.GetProperty("id").GetString()!;
            if (visited.Contains(id))
            {
                break; // focus wrapped around to the first element
            }

            visited.Add(id);
            Assert.NotEqual("none", info.GetProperty("style").GetString());
            Assert.True(info.GetProperty("w").GetDouble() >= 2, $"{id}: vien focus {info.GetProperty("w").GetDouble()}px < 2px");
            Assert.True(info.GetProperty("top").GetDouble() >= 0 && info.GetProperty("bottom").GetDouble() <= info.GetProperty("vh").GetDouble()
                && info.GetProperty("left").GetDouble() >= 0 && info.GetProperty("right").GetDouble() <= info.GetProperty("vw").GetDouble(), $"{id}: ngoai khung nhin");
        }

        Assert.Contains("skip-link", visited);
        Assert.Contains("site-title", visited);
        Assert.Equal(3, visited.Count); // skip link, ten blog, muc Gioi thieu: moi phan tu bam duoc
    }

    [Theory]
    [InlineData("/", 360)]
    [InlineData("/", 1280)]
    [InlineData("/Home/About", 360)]
    [InlineData("/Home/About", 1280)]
    public async Task AC16_Axe_KhongViPham(string path, int width)
    {
        // axe injects a script, so the CSP of the page is bypassed here (see 02-design). AC-43 does not bypass it.
        await using var s = await fx.OpenAsync("chromium", path, width, bypassCsp: true);
        var result = await s.Page.RunAxe(new AxeRunOptions
        {
            RunOnly = new RunOnlyOptions { Type = "tag", Values = ["wcag2a", "wcag2aa", "wcag21aa", "wcag22aa"] },
        });
        var summary = string.Join("; ", result.Violations.Select(v => $"{v.Id}: {v.Help} ({v.Nodes.Length} node)"));
        Assert.True(result.Violations.Length == 0, summary);
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC17_TatJavaScript_MenuBamDuoc(string browser)
    {
        await using var s = await fx.OpenAsync(browser, "/", 360, javaScript: false);
        var link = s.Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = MenuLabel, Exact = true });
        Assert.True(await link.IsVisibleAsync());
        await link.ClickAsync();
        await s.Page.WaitForURLAsync(new Uri(fx.BaseAddress, "/Home/About").ToString());
        Assert.Equal("Lê Khánh Trình", (await s.Page.InnerTextAsync("main h1")).Trim());
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC18_TatJavaScript_TrangGioiThieuDayDu(string browser)
    {
        await using var s = await fx.OpenAsync(browser, "/Home/About", 1280, javaScript: false);
        Assert.True(await s.Page.Locator("main h1").IsVisibleAsync());
        Assert.True(await s.Page.Locator("main p").IsVisibleAsync());
        var text = await s.Page.InnerTextAsync("main");
        Assert.Contains("Lê Khánh Trình", text);
        Assert.Contains("Nhiệm vụ của blog là chia sẻ để giúp cho người nào muốn thay đổi, phát triển, không phân biệt tuổi tác.", text);
    }

    [Theory]
    [MemberData(nameof(BrowserPage))]
    public async Task AC19_FontHeThong_KhongTaiFile(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        var family = await s.StyleAsync("body", "fontFamily");
        Assert.StartsWith("system-ui", family);
        Assert.DoesNotContain(s.Requests, r => r.ResourceType == "font");
        var css = await fx.Http.GetStringAsync(new Uri(fx.BaseAddress, "/css/site.css"));
        var tokens = await fx.Http.GetStringAsync(new Uri(fx.BaseAddress, "/css/tokens.css"));
        Assert.DoesNotContain("@font-face", css + tokens);
    }

    [Theory]
    [MemberData(nameof(BrowserPage))]
    public async Task AC24_MoiYeuCauToiChinhSite(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        Assert.NotEmpty(s.Requests);
        foreach (var r in s.Requests)
        {
            var uri = new Uri(r.Url);
            Assert.True(uri.Scheme is "data" or "blob" or "about" || uri.Authority == fx.BaseAddress.Authority, $"yeu cau ben ngoai: {r.Url}");
        }
    }

    [Theory]
    [MemberData(nameof(BrowserPage))]
    public async Task AC25_KhongConBootstrapJqueryValidation(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        var assets = s.Requests.Where(r => r.ResourceType is "stylesheet" or "script").Select(r => r.Url).ToList();
        Assert.NotEmpty(assets);
        Assert.DoesNotContain(assets, u => Regex.IsMatch(u, "bootstrap|jquery", RegexOptions.IgnoreCase));
        var html = await s.Page.ContentAsync();
        Assert.DoesNotMatch("(?i)bootstrap|jquery", html);
        var globals = await s.Page.EvaluateAsync<bool>("() => typeof window.jQuery !== 'undefined' || typeof window.$ !== 'undefined' || typeof window.bootstrap !== 'undefined'");
        Assert.False(globals);
    }

    [Theory]
    [MemberData(nameof(BrowserPage))]
    public async Task AC43_KhongViPhamCsp(string browser, string path)
    {
        var s = await fx.OpenAsync(browser, path, navigate: false);
        await using (s)
        {
            var messages = new List<string>();
            s.Page.Console += (_, m) => messages.Add(m.Text);
            await s.Page.AddInitScriptAsync("window.__csp = []; document.addEventListener('securitypolicyviolation', e => window.__csp.push(e.violatedDirective + ' ' + e.blockedURI));");
            await s.GotoAsync();
            var violations = await s.Page.EvaluateAsync<string[]>("() => window.__csp");
            Assert.Empty(violations);
            Assert.DoesNotContain(messages, m => Regex.IsMatch(m, "Content.Security.Policy|Refused to", RegexOptions.IgnoreCase));

            // The page must still render fully: the stylesheets applied despite the policy.
            Assert.Equal("#0A58CA", (await s.Page.EvaluateAsync<string>("() => getComputedStyle(document.documentElement).getPropertyValue('--color-primary')")).Trim().ToUpperInvariant());
            Assert.StartsWith("system-ui", await s.StyleAsync("body", "fontFamily"));
        }
    }

    [Theory]
    [MemberData(nameof(BrowserPage))]
    public async Task AC44_TenBlogHienThiDungDauNhay(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        var title = await s.Page.TitleAsync();
        var header = (await s.Page.InnerTextAsync(".site-title")).Trim();
        Assert.Equal(SkeletonFixture.BlogName, header);
        Assert.Contains(SkeletonFixture.BlogName, title);
        foreach (var text in new[] { title, header })
        {
            Assert.DoesNotContain("&#x27;", text);
            Assert.DoesNotContain("&#39;", text);
            Assert.DoesNotContain("&amp;", text);
        }
    }
}
