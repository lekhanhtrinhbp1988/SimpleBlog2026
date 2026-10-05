using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleBlog.Tests.Acceptance.WalkingSkeleton;

// Checks on files and tools (design tokens, Stylelint, CI config, test code) that need no running app.
public partial class ProjectFileTests
{
    private static readonly string Root = SkeletonData.RepoRoot();

    [Fact]
    public void AC26_TokenDungBangDaDuyet()
    {
        var expected = ReadGuidelineTokens();
        var actual = ReadTokensCss();

        Assert.True(expected.Count >= 50, $"chi doc duoc {expected.Count} token tu ui-guidelines.md");
        var problems = new List<string>();
        foreach (var (name, value) in expected)
        {
            if (!actual.TryGetValue(name, out var got))
            {
                problems.Add($"thieu {name}");
            }
            else if (got != value)
            {
                problems.Add($"{name}: file '{got}' != bang '{value}'");
            }
        }

        Assert.Empty(problems);
    }

    [Fact]
    public void AC27_MoiCapMauDuTuongPhan()
    {
        var tokens = ReadTokensCss();
        var failures = new List<string>();

        void Check(string fg, string bg, double min)
        {
            var ratio = Ratio(Rgb(tokens[$"--color-{fg}"]), Rgb(tokens[$"--color-{bg}"]));
            if (ratio < min)
            {
                failures.Add($"--color-{fg} tren --color-{bg}: {ratio:0.00}:1 < {min}:1");
            }
        }

        foreach (var fg in new[] { "text", "text-muted", "primary", "primary-hover", "danger", "success" })
        {
            Check(fg, "bg", 4.5);
        }

        foreach (var fg in new[] { "text", "text-muted", "primary" })
        {
            Check(fg, "surface", 4.5);
        }

        Check("on-primary", "primary", 4.5);
        Check("on-primary", "primary-hover", 4.5);
        foreach (var fg in new[] { "text", "comment", "keyword", "string", "function", "number", "type" })
        {
            Check("code-" + fg, "surface", 4.5);
        }

        Check("focus", "bg", 3);
        Check("border-strong", "bg", 3);
        Assert.Empty(failures);
    }

    [Fact]
    [Trait("Category", "Stylelint")]
    public async Task AC28_CssCuaSiteQuaStylelint()
    {
        var (code, output) = await RunStylelintAsync("src/SimpleBlog.Web/wwwroot/css/**/*.css");
        Assert.True(code == 0, output);
    }

    [Fact]
    [Trait("Category", "Stylelint")]
    public async Task AC29_StylelintChanMauVaCoChuVietCung()
    {
        // A throw-away sample, independent of tests/stylelint/violations.css, outside wwwroot/css/tokens.css.
        var dir = Path.Combine(Path.GetTempPath(), "sb-stylelint-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var file = Path.Combine(dir, "sample.css");
            await File.WriteAllTextAsync(file, "a {\n  color: #123456;\n  font-size: 13px;\n}\n");
            var (code, output) = await RunStylelintAsync(file, "--formatter", "json", "--config", Path.Combine(Root, ".stylelintrc.json"));
            Assert.NotEqual(0, code);
            Assert.Contains("\"rule\":\"color-no-hex\"", output);
            Assert.Contains("\"rule\":\"declaration-property-unit-disallowed-list\"", output);
        }
        finally
        {
            Directory.Delete(dir, true);
        }

        // The sample committed for CI is rejected too.
        var (code2, output2) = await RunStylelintAsync("tests/stylelint/violations.css", "--formatter", "json");
        Assert.NotEqual(0, code2);
        Assert.Contains("color-no-hex", output2);
    }

    [Fact]
    public void AC30_CiCaiBaTrinhDuyetVaChayDotnetTestKhongLoc()
    {
        var ci = File.ReadAllText(Path.Combine(Root, ".github", "workflows", "ci.yml"));
        var install = ci.IndexOf("playwright.ps1 install", StringComparison.Ordinal);
        var test = ci.IndexOf("dotnet test", StringComparison.Ordinal);
        Assert.True(install >= 0, "ci.yml khong cai trinh duyet Playwright");
        Assert.True(test > install, "dotnet test phai chay sau khi cai trinh duyet");
        var installLine = ci.Substring(install, ci.IndexOf('\n', install) - install);
        foreach (var b in new[] { "chromium", "firefox", "webkit" })
        {
            Assert.Contains(b, installLine);
        }

        var testLine = ci.Substring(test, ci.IndexOf('\n', test) - test);
        // Browser tests must still run; the only allowed exclusion is Category=Stylelint (it runs in the stylelint job).
        Assert.DoesNotContain("Browser", testLine);
        var filters = Regex.Matches(testLine, @"--filter\s+""?([^""\s]+)""?");
        Assert.True(filters.Count <= 1, "dotnet test co nhieu hon mot --filter");
        if (filters.Count == 1)
        {
            Assert.Equal("Category!=Stylelint", filters[0].Groups[1].Value);
        }

        Assert.DoesNotContain("continue-on-error", ci);
    }

    [Fact]
    public void AC31_CiCoJobStylelintDoKhiViPham()
    {
        var ci = File.ReadAllText(Path.Combine(Root, ".github", "workflows", "ci.yml"));
        var job = ci.IndexOf("  stylelint:", StringComparison.Ordinal);
        Assert.True(job >= 0, "ci.yml khong co job stylelint");
        var body = ci[job..];
        Assert.Contains("npx stylelint", body);
        var testAt = body.IndexOf("dotnet test", StringComparison.Ordinal);
        Assert.True(testAt >= 0, "job stylelint khong chay dotnet test");
        var stylelintTestLine = body.Substring(testAt, body.IndexOf('\n', testAt) - testAt);
        Assert.Matches(@"--filter\s+""?Category=Stylelint""?", stylelintTestLine);
        Assert.DoesNotContain("continue-on-error", ci);
        Assert.DoesNotContain("|| true", ci);
        Assert.Contains("branches: [main]", ci);
        Assert.Contains("pull_request", ci);
    }

    [Fact]
    public void AC35_AcceptanceTestDungTrucTiepLopProgram()
    {
        // Compile-time proof: WebApplicationFactory<Program> is used directly by SkeletonFixture.
        Assert.True(typeof(Program).IsPublic);

        var offenders = Directory.EnumerateFiles(Path.Combine(Root, "tests", "SimpleBlog.Tests"), "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                && !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                && !f.EndsWith("ProjectFileTests.cs", StringComparison.Ordinal))
            .Where(f => Regex.IsMatch(File.ReadAllText(f), @"MakeGenericType|GetType\(""Program""\)|Activator\.CreateInstance"))
            .ToList();
        Assert.Empty(offenders);
    }

    private static async Task<(int Code, string Output)> RunStylelintAsync(params string[] args)
    {
        var script = Path.Combine(Root, "node_modules", "stylelint", "bin", "stylelint.mjs");
        Assert.True(File.Exists(script), "chua co node_modules/stylelint: chay `npm install` o goc repo");
        var psi = new ProcessStartInfo("node")
        {
            WorkingDirectory = Root,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };
        psi.ArgumentList.Add(script);
        foreach (var a in args)
        {
            psi.ArgumentList.Add(a);
        }

        using var p = Process.Start(psi)!;
        var stdout = p.StandardOutput.ReadToEndAsync();
        var stderr = p.StandardError.ReadToEndAsync();
        await p.WaitForExitAsync();
        return (p.ExitCode, await stdout + await stderr);
    }

    private static Dictionary<string, string> ReadTokensCss()
    {
        var css = File.ReadAllText(Path.Combine(Root, "src", "SimpleBlog.Web", "wwwroot", "css", "tokens.css"));
        var d = new Dictionary<string, string>();
        foreach (Match m in DeclarationRegex().Matches(css))
        {
            d[m.Groups[1].Value] = Normalize(m.Groups[2].Value);
        }

        return d;
    }

    // Reads `| `--token` | `value` | ...` rows from the "Design tokens" section of ui-guidelines.md.
    private static Dictionary<string, string> ReadGuidelineTokens()
    {
        var text = File.ReadAllText(Path.Combine(Root, "docs", "project", "ui-guidelines.md"));
        var start = text.IndexOf("## Design tokens", StringComparison.Ordinal);
        var end = text.IndexOf("## Layout", StringComparison.Ordinal);
        var d = new Dictionary<string, string>();
        foreach (Match m in RowRegex().Matches(text[start..end]))
        {
            d[m.Groups[1].Value] = Normalize(m.Groups[2].Value);
        }

        return d;
    }

    private static string Normalize(string v) => Regex.Replace(v.Trim(), @"\s+", " ");

    private static (double R, double G, double B) Rgb(string hex)
    {
        Assert.Matches("^#[0-9A-Fa-f]{6}$", hex);
        return (
            int.Parse(hex[1..3], NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0,
            int.Parse(hex[3..5], NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0,
            int.Parse(hex[5..7], NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0);
    }

    private static double Luminance((double R, double G, double B) c)
    {
        static double L(double v) => v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        return (0.2126 * L(c.R)) + (0.7152 * L(c.G)) + (0.0722 * L(c.B));
    }

    private static double Ratio((double R, double G, double B) a, (double R, double G, double B) b)
    {
        var la = Luminance(a);
        var lb = Luminance(b);
        return (Math.Max(la, lb) + 0.05) / (Math.Min(la, lb) + 0.05);
    }

    [GeneratedRegex(@"(--[a-z0-9-]+)\s*:\s*([^;]+);")]
    private static partial Regex DeclarationRegex();

    [GeneratedRegex(@"^\|\s*`(--[a-z0-9-]+)`\s*\|\s*`([^`]+)`", RegexOptions.Multiline)]
    private static partial Regex RowRegex();
}
