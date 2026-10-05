using System.Net;
using System.Text.RegularExpressions;

namespace SimpleBlog.Tests.Acceptance.WalkingSkeleton;

[Collection(SkeletonTestGroup.Name)]
public class HttpTests(SkeletonFixture fx)
{
    private static readonly string[] Pages = ["/", "/Home/About"];

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/About")]
    public async Task AC6_MenuChiCoMucGioiThieu(string path)
    {
        var (r, html) = await fx.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        var nav = Regex.Match(html, @"<nav[^>]*>(.*?)</nav>", RegexOptions.Singleline);
        Assert.True(nav.Success, "khong co nav");
        var links = Regex.Matches(nav.Groups[1].Value, @"<a\b[^>]*>(.*?)</a>", RegexOptions.Singleline);
        Assert.Single(links);
        Assert.Equal("Giới thiệu", SkeletonData.Decode(links[0].Groups[1].Value).Trim());
        var visibleText = SkeletonData.Decode(Regex.Replace(nav.Groups[1].Value, "<[^>]+>", " "));
        foreach (var banned in new[] { "Bài viết", "Thẻ", "Tìm kiếm", "Home", "Privacy" })
        {
            Assert.DoesNotContain(banned, visibleText);
        }
    }

    [Fact]
    public async Task AC33_TrangChu_TieuDeVaCauGiaiThich()
    {
        var (r, html) = await fx.GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        var h1 = Regex.Matches(html, @"<h1[^>]*>(.*?)</h1>", RegexOptions.Singleline);
        Assert.Single(h1);
        Assert.Equal("Bài viết", SkeletonData.Decode(h1[0].Groups[1].Value).Trim());
        var text = SkeletonData.Decode(html);
        Assert.Contains("Chưa có bài viết nào.", text);
        Assert.DoesNotContain("Welcome", text);
        Assert.DoesNotContain("learn.microsoft.com", text);
    }

    [Theory]
    [InlineData("/Home/Privacy")]
    [InlineData("/Privacy")]
    public async Task AC34_TrangPrivacyCuaKhungMau_Tra404(string path)
    {
        var (r, _) = await fx.GetAsync(path);
        Assert.Equal(HttpStatusCode.NotFound, r.StatusCode);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/About")]
    public async Task AC37_LangLaVi(string path)
    {
        var (_, html) = await fx.GetAsync(path);
        Assert.Matches(@"<html[^>]*\slang=""vi""", html);
    }

    [Fact]
    public async Task AC38_TieuDeTrinhDuyetRiengTungTrang()
    {
        var (_, home) = await fx.GetAsync("/");
        var (_, about) = await fx.GetAsync("/Home/About");
        Assert.Equal("Trình's Dev Notes", SkeletonData.Title(home));
        Assert.Equal("Giới thiệu - Trình's Dev Notes", SkeletonData.Title(about));
    }

    [Fact]
    public async Task AC39_MoTaRiengTungTrang()
    {
        var (_, home) = await fx.GetAsync("/");
        var (_, about) = await fx.GetAsync("/Home/About");
        var homeDesc = SkeletonData.MetaDescription(home);
        var aboutDesc = SkeletonData.MetaDescription(about);
        Assert.False(string.IsNullOrWhiteSpace(homeDesc), "trang chu thieu meta description");
        Assert.False(string.IsNullOrWhiteSpace(aboutDesc), "trang gioi thieu thieu meta description");
        Assert.NotEqual(homeDesc, aboutDesc);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/About")]
    [InlineData("/css/site.css")]
    public async Task AC40_NoSniff(string path)
    {
        var (r, _) = await fx.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        Assert.Equal(["nosniff"], r.Headers.GetValues("X-Content-Type-Options"));
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/About")]
    public async Task AC41_ReferrerPolicy(string path)
    {
        var (r, _) = await fx.GetAsync(path);
        Assert.Equal(["strict-origin-when-cross-origin"], r.Headers.GetValues("Referrer-Policy"));
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/About")]
    public async Task AC42_CspKhongChoScriptInline(string path)
    {
        var (r, html) = await fx.GetAsync(path);
        Assert.True(r.Headers.TryGetValues("Content-Security-Policy", out var values), "thieu header Content-Security-Policy");
        var csp = Assert.Single(values);
        Assert.DoesNotContain("'unsafe-inline'", csp);
        Assert.DoesNotContain("'unsafe-eval'", csp);
        Assert.DoesNotContain("data:", csp);
        Assert.DoesNotContain("*", csp);

        var directives = csp.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(d => d.Split(' ', 2))
            .ToDictionary(p => p[0], p => p.Length > 1 ? p[1] : string.Empty);
        foreach (var name in new[] { "script-src", "style-src", "font-src", "connect-src" })
        {
            Assert.True(directives.TryGetValue(name, out var v), $"thieu {name}");
            Assert.Equal("'self'", v);
        }

        // Page content matches the policy: no inline script or style in the HTML.
        Assert.DoesNotMatch(@"<script(?![^>]*\ssrc=)", html);
        Assert.DoesNotMatch(@"\sstyle\s*=", html);
        Assert.DoesNotMatch(@"<style\b", html);
    }
}
