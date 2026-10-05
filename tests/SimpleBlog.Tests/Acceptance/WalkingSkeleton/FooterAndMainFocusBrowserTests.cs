using System.Text.Json;
using Microsoft.Playwright;

namespace SimpleBlog.Tests.Acceptance.WalkingSkeleton;

[Collection(SkeletonTestGroup.Name)]
[Trait("Category", "Browser")]
public class FooterAndMainFocusBrowserTests(SkeletonFixture fx)
{
    public static TheoryData<string, int> HomeViewports()
    {
        var data = new TheoryData<string, int>();
        foreach (var b in SkeletonData.Browsers)
        {
            foreach (var w in new[] { 360, 1280 })
            {
                data.Add(b, w);
            }
        }

        return data;
    }

    public static TheoryData<string, int, string, int> FooterOverlapMatrix()
    {
        var data = new TheoryData<string, int, string, int>();
        foreach (var b in SkeletonData.Browsers)
        {
            foreach (var w in new[] { 360, 1280 })
            {
                foreach (var p in SkeletonData.Pages)
                {
                    foreach (var h in new[] { 1600, 200 })
                    {
                        data.Add(b, w, p, h);
                    }
                }
            }
        }

        return data;
    }

    public static TheoryData<string, string> KeyboardBrowserPage() => InteractionBrowserTests.KeyboardBrowserPage();

    [Theory]
    [MemberData(nameof(HomeViewports))]
    public async Task AC45_NoiDungNgan_FooterSatDayKhungNhin(string browser, int width)
    {
        await using var s = await fx.OpenAsync(browser, "/", width, 800);
        var footer = await s.RectAsync("footer");
        var vh = await s.Page.EvaluateAsync<double>("() => innerHeight");
        Assert.Equal(0, await s.Page.EvaluateAsync<double>("() => scrollY"));
        Assert.True(Math.Abs(footer.Bottom - vh) <= 1, $"canh duoi footer {footer.Bottom}, canh duoi khung nhin {vh}");
    }

    [Theory]
    [MemberData(nameof(HomeViewports))]
    public async Task AC46_NoiDungDai_FooterNamNgoaiKhungNhin_VaKhongDinh(string browser, int width)
    {
        await using var s = await fx.OpenAsync(browser, "/Home/About", width, 150);
        var vh = await s.Page.EvaluateAsync<double>("() => innerHeight");
        var main = await s.RectAsync("main#main");
        Assert.True(main.Bottom > vh, "dieu kien thu: noi dung phai cao hon khung nhin");
        var footer = await s.RectAsync("footer");
        Assert.True(footer.Top >= vh - 0.5, $"footer nam trong khung nhin: top {footer.Top}, vh {vh}");

        // Not sticky: after scrolling it moves with the page instead of staying at the bottom edge.
        await s.Page.EvaluateAsync("() => window.scrollTo(0, 60)");
        var scrolled = await s.RectAsync("footer");
        Assert.True(scrolled.Top < footer.Top - 30, $"footer khong cuon theo trang: truoc {footer.Top}, sau {scrolled.Top}");
    }

    [Theory]
    [MemberData(nameof(FooterOverlapMatrix))]
    public async Task AC47_FooterKhongChongLenNoiDung(string browser, int width, string path, int height)
    {
        await using var s = await fx.OpenAsync(browser, path, width, height);
        var main = await s.RectAsync("main#main");
        var footer = await s.RectAsync("footer");
        Assert.True(footer.Top >= main.Bottom - 0.5, $"footer chong len main: footer.top {footer.Top}, main.bottom {main.Bottom} (khung nhin {width}x{height})");
    }

    private static readonly string MainVisualState =
        """
        () => { const e = document.getElementById('main'); const cs = getComputedStyle(e);
          return { active: document.activeElement === e,
                   outlineStyle: cs.outlineStyle, outlineWidth: parseFloat(cs.outlineWidth),
                   boxShadow: cs.boxShadow,
                   border: [cs.borderTopWidth, cs.borderRightWidth, cs.borderBottomWidth, cs.borderLeftWidth].map(parseFloat) }; }
        """;

    private static void AssertNoRing(JsonElement info)
    {
        Assert.True(info.GetProperty("active").GetBoolean(), "main khong nhan focus");
        var outlineVisible = info.GetProperty("outlineStyle").GetString() != "none" && info.GetProperty("outlineWidth").GetDouble() > 0;
        Assert.False(outlineVisible, $"main co outline {info.GetProperty("outlineStyle").GetString()} {info.GetProperty("outlineWidth").GetDouble()}px");
        Assert.Equal("none", info.GetProperty("boxShadow").GetString());
        foreach (var w in info.GetProperty("border").EnumerateArray())
        {
            Assert.Equal(0, w.GetDouble());
        }
    }

    [Theory]
    [MemberData(nameof(KeyboardBrowserPage))]
    public async Task AC48_SauLienKetBoQua_MainKhongVeVongFocus_BanPhim(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        await s.Page.Keyboard.PressAsync("Tab");
        await s.Page.Keyboard.PressAsync("Enter");
        await s.Page.WaitForFunctionAsync("() => document.activeElement && document.activeElement.id === 'main'", null, new PageWaitForFunctionOptions { Timeout = 3000 });
        AssertNoRing(await s.Page.EvaluateAsync<JsonElement>(MainVisualState));
    }

    [Theory]
    [MemberData(nameof(SkeletonData.BrowserPage), MemberType = typeof(SkeletonData))]
    public async Task AC48_SauLienKetBoQua_MainKhongVeVongFocus_ChuotVaFocusLapTrinh(string browser, string path)
    {
        await using var s = await fx.OpenAsync(browser, path);
        // Moving focus to main as the skip link does (works in WebKit too, where Tab skips links on Windows/macOS).
        await s.Page.EvaluateAsync("() => { document.querySelector('.skip-link').focus(); document.getElementById('main').focus(); }");
        AssertNoRing(await s.Page.EvaluateAsync<JsonElement>(MainVisualState));

        // Links still show a focus ring (AC-14 scope): focus the site title and check its outline.
        await s.Page.Keyboard.PressAsync("Tab");
        await s.Page.EvaluateAsync("() => document.querySelector('.site-title').focus()");
        var title = await s.Page.EvaluateAsync<JsonElement>("() => { const cs = getComputedStyle(document.querySelector('.site-title')); return { s: cs.outlineStyle, w: parseFloat(cs.outlineWidth) }; }");
        Assert.NotEqual("none", title.GetProperty("s").GetString());
        Assert.True(title.GetProperty("w").GetDouble() >= 2);
    }
}
