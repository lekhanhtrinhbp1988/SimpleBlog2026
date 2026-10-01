using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SimpleBlog.Tests.Acceptance.About;

// Program (top-level statements) la internal nen khong dung duoc WebApplicationFactory<Program>
// truc tiep tu assembly test; tao factory bang reflection tren cung kieu Program do.
public sealed class WebFactoryHolder : IDisposable
{
    private readonly IDisposable _factory;
    public WebFactoryHolder()
    {
        var programType = typeof(SimpleBlog.Web.Controllers.HomeController).Assembly.GetType("Program")
            ?? throw new InvalidOperationException("Khong tim thay kieu Program");
        var factoryType = typeof(WebApplicationFactory<>).MakeGenericType(programType);
        _factory = (IDisposable)Activator.CreateInstance(factoryType)!;
        var create = factoryType.GetMethods().First(m => m.Name == "CreateClient" && m.GetParameters().Length == 1);
        var options = new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        };
        Client = (HttpClient)create.Invoke(_factory, new object[] { options })!;
    }
    public HttpClient Client { get; }
    public void Dispose() { Client.Dispose(); _factory.Dispose(); }
}

public class AboutAcceptanceTests : IClassFixture<WebFactoryHolder>
{
    private const string BlogName = "Lê Khánh Trình";
    private const string Description = "Nhiệm vụ của blog là chia sẻ để giúp cho người nào muốn thay đổi, phát triển, không phân biệt tuổi tác.";
    private const string MenuLabel = "Giới thiệu";
    private readonly HttpClient _client;

    public AboutAcceptanceTests(WebFactoryHolder f) => _client = f.Client;

    private async Task<string> GetOk(string url)
    {
        var r = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        var bytes = await r.Content.ReadAsByteArrayAsync();
        return Encoding.UTF8.GetString(bytes).Normalize(NormalizationForm.FormC);
    }

    private static void AssertMenuHasGioiThieu(string html)
    {
        var navStart = html.IndexOf("class=\"navbar-nav", StringComparison.Ordinal);
        Assert.True(navStart >= 0, "Khong thay menu chinh");
        var navEnd = html.IndexOf("</ul>", navStart, StringComparison.Ordinal);
        var nav = html.Substring(navStart, navEnd - navStart);
        Assert.Matches(@"<a[^>]*>\s*" + MenuLabel + @"\s*</a>", nav);
    }

    [Fact]
    public async Task AC1_TrangChu_MenuCoMucGioiThieu()
        => AssertMenuHasGioiThieu(await GetOk("/"));

    [Fact]
    public async Task AC2_BamMucMenu_MoTrangGioiThieuThanhCong()
    {
        var html = await GetOk("/");
        var m = System.Text.RegularExpressions.Regex.Match(html, @"<a[^>]*href=""([^""]+)""[^>]*>\s*" + MenuLabel + @"\s*</a>");
        Assert.True(m.Success, "Khong thay link Giới thiệu");
        var about = await GetOk(m.Groups[1].Value);
        Assert.Contains(BlogName, about);
    }

    [Fact]
    public async Task AC3_TrangGioiThieu_HienThiTenBlog()
        => Assert.Contains(BlogName, await GetOk("/Home/About"));

    [Fact]
    public async Task AC4_TrangGioiThieu_HienThiMoTa()
        => Assert.Contains(Description, await GetOk("/Home/About"));

    [Fact]
    public async Task AC5_TrangGioiThieu_MenuCoMucGioiThieu()
        => AssertMenuHasGioiThieu(await GetOk("/Home/About"));

    [Fact]
    public async Task AC6_MoTrucTiep_KhongCanDangNhap()
    {
        using var fresh = new WebFactoryHolder();
        var r = await fresh.Client.GetAsync("/Home/About");
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        Assert.False(r.Headers.Contains("Location"));
        Assert.Contains(BlogName, await r.Content.ReadAsStringAsync());
    }
}
