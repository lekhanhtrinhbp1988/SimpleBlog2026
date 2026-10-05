using System.Globalization;
using System.Text.Json;
using Microsoft.Playwright;

namespace SimpleBlog.Tests.Acceptance.WalkingSkeleton;

[Collection(SkeletonTestGroup.Name)]
[Trait("Category", "Browser")]
public class LayoutBrowserTests(SkeletonFixture fx)
{
    public static TheoryData<string, int, string> All() => SkeletonData.All();

    public static TheoryData<string, int, string> WideWidths()
        => SkeletonData.Matrix(["chromium", "firefox", "webkit"], [1280, 1920], SkeletonData.Pages);

    public static TheoryData<string, int, string> HeaderRowWidths()
        => SkeletonData.Matrix(["chromium", "firefox", "webkit"], [768, 1280, 1920], SkeletonData.Pages);

    public static TheoryData<string, int, string> NarrowWidths()
        => SkeletonData.Matrix(["chromium", "firefox", "webkit"], [320, 360], SkeletonData.Pages);

    public static TheoryData<string, int, string> TouchWidths()
        => SkeletonData.Matrix(["chromium", "firefox", "webkit"], [360, 1280], SkeletonData.Pages);

    [Theory]
    [MemberData(nameof(WideWidths))]
    public async Task AC1_MotCotToiDa720px_CanGiua(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        foreach (var selector in new[] { "header .container", "main#main", "footer .container" })
        {
            var r = await s.RectAsync(selector);
            Assert.True(r.Width <= 720.5, $"{selector} rong {r.Width}px > 720px");
            var leftGap = r.Left;
            var rightGap = width - r.Right;
            Assert.True(Math.Abs(leftGap - rightGap) <= 1.5, $"{selector} khong can giua: trai {leftGap}, phai {rightGap}");
        }
    }

    [Theory]
    [MemberData(nameof(All))]
    public async Task AC2_KhongCoCotThuHai(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        var asides = await s.Page.EvaluateAsync<int>("() => document.querySelectorAll('aside, [role=complementary]').length");
        Assert.Equal(0, asides);
        Assert.Equal(1, await s.Page.EvaluateAsync<int>("() => document.querySelectorAll('main').length"));

        // Header, main and footer must be stacked vertically, nothing sits beside the main content.
        var header = await s.RectAsync("header");
        var main = await s.RectAsync("main#main");
        var footer = await s.RectAsync("footer");
        Assert.True(header.Bottom <= main.Top + 0.5, "header chong len main");
        Assert.True(main.Bottom <= footer.Top + 0.5, "main chong len footer");

        // No visible element (other than the off-screen skip link) overlaps the vertical span of main
        // outside main's own horizontal extent.
        var beside = await s.Page.EvaluateAsync<int>(
            """
            () => {
              const m = document.querySelector('main#main').getBoundingClientRect();
              let n = 0;
              for (const e of document.body.children) {
                if (e.matches('main') || e.matches('.skip-link')) continue;
                const r = e.getBoundingClientRect();
                if (r.width > 0 && r.height > 0 && r.top < m.bottom - 0.5 && r.bottom > m.top + 0.5) n++;
              }
              return n;
            }
            """);
        Assert.Equal(0, beside);
    }

    [Theory]
    [MemberData(nameof(All))]
    public async Task AC3_KhongCuonNgang(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        var m = await s.Page.EvaluateAsync<double[]>(
            "() => [document.documentElement.scrollWidth, document.documentElement.clientWidth]");
        Assert.True(m[0] <= m[1], $"scrollWidth {m[0]} > clientWidth {m[1]}");
    }

    [Theory]
    [MemberData(nameof(HeaderRowWidths))]
    public async Task AC8_HeaderMotHang_TenBlogTraiMenuPhai(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        var title = await s.RectAsync(".site-title");
        var nav = await s.RectAsync("nav[aria-label=\"Menu chính\"]");
        Assert.True(nav.Left >= title.Right - 0.5, "menu khong nam ben phai ten blog");
        Assert.True(title.Top < nav.Bottom && nav.Top < title.Bottom, "ten blog va menu khong cung mot hang");
        var container = await s.RectAsync("header .container");
        Assert.True(nav.Left > container.Left + container.Width / 2, "menu khong nam o nua ben phai");
    }

    [Theory]
    [MemberData(nameof(NarrowWidths))]
    public async Task AC9_HeaderXuongDong_KhongCoNutBaGach(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        var title = await s.RectAsync(".site-title");
        var nav = await s.RectAsync("nav[aria-label=\"Menu chính\"]");
        Assert.True(nav.Top >= title.Bottom - 0.5, $"menu khong nam duoi ten blog: menu.top {nav.Top}, title.bottom {title.Bottom}");
        var buttons = await s.Page.EvaluateAsync<int>("() => document.querySelectorAll('header button, header [role=button], header summary, header input').length");
        Assert.Equal(0, buttons);
    }

    [Theory]
    [MemberData(nameof(SkeletonData.PageOnly), MemberType = typeof(SkeletonData))]
    public async Task AC10_HeaderKhongDinhKhiCuon(string path)
    {
        await using var s = await fx.OpenAsync("chromium", path, 320, 150);
        var tall = await s.Page.EvaluateAsync<bool>("() => document.documentElement.scrollHeight > window.innerHeight");
        Assert.True(tall, "trang khong dai hon khung nhin, khong kiem duoc");
        await s.Page.EvaluateAsync("() => window.scrollTo(0, document.documentElement.scrollHeight)");
        var header = await s.RectAsync("header");
        Assert.True(header.Bottom <= 0, $"header con trong khung nhin: bottom {header.Bottom}");
        Assert.NotEqual("sticky", await s.StyleAsync("header", "position"));
        Assert.NotEqual("fixed", await s.StyleAsync("header", "position"));
    }

    [Theory]
    [MemberData(nameof(TouchWidths))]
    public async Task AC15_VungBamDuLon(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        var menu = await s.Page.EvaluateAsync<double[]>(
            "() => [...document.querySelectorAll('nav a')].map(a => a.getBoundingClientRect().height)");
        Assert.NotEmpty(menu);
        Assert.All(menu, h => Assert.True(h >= 44, $"muc menu cao {h}px < 44px"));

        var others = await s.Page.EvaluateAsync<JsonElement>(
            """
            () => [...document.querySelectorAll('header a, footer a, .skip-link')]
              .filter(a => !a.closest('nav'))
              .map(a => { const r = a.getBoundingClientRect(); return [r.width, r.height]; })
            """);
        foreach (var item in others.EnumerateArray())
        {
            Assert.True(item[0].GetDouble() >= 24 && item[1].GetDouble() >= 24, $"lien ket {item[0].GetDouble()}x{item[1].GetDouble()} < 24px");
        }
    }

    [Theory]
    [MemberData(nameof(All))]
    public async Task AC20_CoChuGocLa16px(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        Assert.Equal("16px", await s.StyleAsync("html", "fontSize"));
    }

    [Theory]
    [MemberData(nameof(TouchWidths))]
    public async Task AC21_KhongCoChuNhoHon14px(string browser, int width, string path)
    {
        await using var s = await fx.OpenAsync(browser, path, width);
        var small = await s.Page.EvaluateAsync<string[]>(
            """
            () => {
              const bad = [];
              const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
              let n;
              while ((n = walker.nextNode())) {
                if (!n.textContent.trim()) continue;
                const e = n.parentElement;
                const cs = getComputedStyle(e);
                if (cs.display === 'none' || cs.visibility === 'hidden') continue;
                const size = parseFloat(cs.fontSize);
                if (size < 14) bad.push(e.tagName + '.' + e.className + ' ' + size + 'px');
              }
              return bad;
            }
            """);
        Assert.Empty(small);
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC22_Doan18px_DongThua(string browser)
    {
        await using var s = await fx.OpenAsync(browser, "/Home/About");
        var info = await s.Page.EvaluateAsync<double[]>(
            "() => { const cs = getComputedStyle(document.querySelector('main p')); return [parseFloat(cs.fontSize), parseFloat(cs.lineHeight)]; }");
        Assert.Equal(18, info[0]);
        Assert.True(info[1] / info[0] >= 1.7 - 0.001, $"line-height {info[1]}px / {info[0]}px = {info[1] / info[0]}");
    }

    [Theory]
    [InlineData("chromium")]
    [InlineData("firefox")]
    [InlineData("webkit")]
    public async Task AC23_TieuDeTheoBreakpoint(string browser)
    {
        await using (var wide = await fx.OpenAsync(browser, "/Home/About", 1280))
        {
            Assert.Equal("36px", await wide.StyleAsync("main h1", "fontSize"));
            Assert.Equal("700", await wide.StyleAsync("main h1", "fontWeight"));
        }

        await using var narrow = await fx.OpenAsync(browser, "/Home/About", 360);
        Assert.Equal("30px", await narrow.StyleAsync("main h1", "fontSize"));
        Assert.Equal("700", await narrow.StyleAsync("main h1", "fontWeight"));
    }

    [Theory]
    [MemberData(nameof(SkeletonData.BrowserPage), MemberType = typeof(SkeletonData))]
    public async Task AC36_DongBanQuyen_14px_MauMuted(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        var text = (await s.Page.InnerTextAsync("footer")).Trim().Normalize(System.Text.NormalizationForm.FormC);
        Assert.Contains($"© {DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)} Lê Khánh Trình", text);
        Assert.Equal("14px", await s.StyleAsync(".site-footer__copy", "fontSize"));
        Assert.Equal("rgb(89, 99, 110)", await s.StyleAsync(".site-footer__copy", "color"));
    }
}
