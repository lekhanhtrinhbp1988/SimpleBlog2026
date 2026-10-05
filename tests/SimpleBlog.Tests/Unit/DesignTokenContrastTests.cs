using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleBlog.Tests.Unit;

public static partial class DesignTokenContrastTests
{
    private static readonly Lazy<Dictionary<string, string>> Tokens = new(LoadTokens);

    public static TheoryData<string, string, double> Pairs()
    {
        var data = new TheoryData<string, string, double>();

        foreach (var fg in new[] { "text", "text-muted", "primary", "primary-hover", "danger", "success" })
        {
            data.Add(fg, "bg", 4.5);
        }

        foreach (var fg in new[] { "text", "text-muted", "primary" })
        {
            data.Add(fg, "surface", 4.5);
        }

        data.Add("on-primary", "primary", 4.5);
        data.Add("on-primary", "primary-hover", 4.5);

        foreach (var fg in new[] { "text", "comment", "keyword", "string", "function", "number", "type" })
        {
            data.Add("code-" + fg, "surface", 4.5);
        }

        data.Add("focus", "bg", 3.0);
        data.Add("border-strong", "bg", 3.0);
        return data;
    }

    [Theory]
    [MemberData(nameof(Pairs))]
    public static void Pair_MeetsMinimumContrast(string foreground, string background, double minimum)
    {
        var fg = Hex(foreground);
        var bg = Hex(background);

        var ratio = Ratio(fg, bg);

        Assert.True(ratio >= minimum, $"--color-{foreground} on --color-{background}: {ratio:0.00}:1 < {minimum}:1");
    }

    [Fact]
    public static void TokensFile_DeclaresAllTokensOfTheFourTables()
    {
        // 12 colours + 7 code colours + 17 type + 18 spacing/radius/misc
        Assert.Equal(54, AllDeclarations().Count);
    }

    private static string Hex(string name)
    {
        Assert.True(Tokens.Value.TryGetValue("--color-" + name, out var value), $"missing --color-{name}");
        Assert.Matches("^#[0-9A-F]{6}$", value);
        return value!;
    }

    private static double Ratio(string a, string b)
    {
        var la = Luminance(a);
        var lb = Luminance(b);
        return (Math.Max(la, lb) + 0.05) / (Math.Min(la, lb) + 0.05);
    }

    private static double Luminance(string hex)
    {
        double Channel(int start)
        {
            var c = int.Parse(hex.AsSpan(start, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
            return c <= 0.03928 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        return (0.2126 * Channel(1)) + (0.7152 * Channel(3)) + (0.0722 * Channel(5));
    }

    private static Dictionary<string, string> LoadTokens()
    {
        return AllDeclarations();
    }

    private static Dictionary<string, string> AllDeclarations()
    {
        var path = Path.Combine(RepoRoot(), "src", "SimpleBlog.Web", "wwwroot", "css", "tokens.css");
        var css = File.ReadAllText(path);
        var result = new Dictionary<string, string>();
        foreach (Match m in DeclarationRegex().Matches(css))
        {
            result[m.Groups[1].Value] = m.Groups[2].Value.Trim();
        }

        return result;
    }

    private static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "SimpleBlog.slnx")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("SimpleBlog.slnx not found");
    }

    [GeneratedRegex(@"(--[a-z0-9-]+)\s*:\s*([^;]+);")]
    private static partial Regex DeclarationRegex();
}
