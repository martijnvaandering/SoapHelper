using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Xunit.Sdk;

namespace SoapHelper.Tests
{
    public class UrlTests
    {
        [Theory(DisplayName = "Url test")]
        [InlineData("http://foo.com/blah_blah", 1)]
        [InlineData("http://foo.com/blah_blah_(wikipedia", 1)]
        [InlineData("http://foo.com/blah_blah_(wikipedia)_(again", 1)]
        [InlineData("http://www.example.com/wpstyle/?p=364", 1)]
        [InlineData("https://www.example.com/foo/?bar=baz&inga=42&quux", 1)]
        [InlineData("http://✪df.ws/123", 0)]
        [InlineData("http://userid:password@example.com:8080", 1)]
        [InlineData("http://userid@example.com", 1)]
        [InlineData("http://userid@example.com:8080", 1)]
        [InlineData("http://userid:password@example.com", 1)]
        [InlineData("http://142.42.1.1", 1)]
        [InlineData("http://142.42.1.1:8080", 1)]
        [InlineData("http://➡.ws/䨹", 0)]
        [InlineData("http://⌘.ws", 0)]
        [InlineData("http://foo.com/blah_(wikipedia)#cite-1", 1)]
        [InlineData("http://foo.com/blah_(wikipedia)_blah#cite-1", 1)]
        [InlineData("http://foo.com/unicode_(✪)_in_parens", 1)]
        [InlineData("http://foo.com/(something)?after=parens", 1)]
        [InlineData("http://☺.damowmow.com", 1)]
        [InlineData("http://code.google.com/events/#&product=browser", 1)]
        [InlineData("http://j.mp", 1)]
        [InlineData("ftp://foo.bar/baz", 0)]
        [InlineData("http://foo.bar/?q=Test%20URL-encoded%20stuff", 1)]
        [InlineData("http://-.~_!$&'()*+,;=:%40:80%2f::::::@example.com", 1)]
        [InlineData("http://1337.net", 1)]
        [InlineData("http://a.b-c.de", 1)]
        [InlineData("http://223.255.255.254", 1)]
        public void TestUrls(string url, int outcome)
        {
            Xunit.Assert.True(HttpUrlValidation.IsValidHttpAddress(url) == (outcome == 1));
        }
    }
}
