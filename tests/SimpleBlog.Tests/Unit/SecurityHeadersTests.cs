using Microsoft.AspNetCore.Http;
using SimpleBlog.Web.Services;

namespace SimpleBlog.Tests.Unit;

public class SecurityHeadersTests
{
    [Fact]
    public void Apply_SetsThreeHeaders()
    {
        var headers = new HeaderDictionary();

        SecurityHeaders.Apply(headers);

        Assert.Equal(SecurityHeaders.ContentSecurityPolicy, headers["Content-Security-Policy"].ToString());
        Assert.Equal("nosniff", headers["X-Content-Type-Options"].ToString());
        Assert.Equal("strict-origin-when-cross-origin", headers["Referrer-Policy"].ToString());
    }

    [Theory]
    [InlineData("script-src 'self'")]
    [InlineData("style-src 'self'")]
    [InlineData("font-src 'self'")]
    [InlineData("connect-src 'self'")]
    public void ContentSecurityPolicy_ContainsDirective(string directive)
    {
        Assert.Contains(directive, SecurityHeaders.ContentSecurityPolicy, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("unsafe-inline")]
    [InlineData("unsafe-eval")]
    [InlineData("upgrade-insecure-requests")]
    public void ContentSecurityPolicy_DoesNotContain(string text)
    {
        Assert.DoesNotContain(text, SecurityHeaders.ContentSecurityPolicy, StringComparison.Ordinal);
    }

    [Fact]
    public void Apply_CalledTwice_KeepsSingleValuePerHeader()
    {
        var headers = new HeaderDictionary();

        SecurityHeaders.Apply(headers);
        SecurityHeaders.Apply(headers);

        Assert.Single(headers["Content-Security-Policy"]);
        Assert.Single(headers["X-Content-Type-Options"]);
        Assert.Single(headers["Referrer-Policy"]);
    }
}
