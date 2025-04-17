using Bhbk.Lib.Waf.HttpOption;
using System;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.HttpOption
{
    public class HttpsTests
    {
        [Fact]
        public void HttpsNotAllowedMatch()
        {
            Assert.True(CheckActionFilterHttp(Constants.TestUri_1, HttpFilterAction.SslNotAllowed));
        }

        [Fact]
        public void HttpsNotAllowedNoMatch()
        {
            Assert.False(CheckActionFilterHttp(Constants.TestUri_2, HttpFilterAction.SslNotAllowed));
        }

        [Fact]
        public void HttpsOptionalMatch()
        {
            Assert.True(CheckActionFilterHttp(Constants.TestUri_1, HttpFilterAction.SslOptional));
        }

        [Fact]
        public void HttpsOptionalNoMatch()
        {
            Assert.False(CheckActionFilterHttp(Constants.TestUri_3, HttpFilterAction.SslOptional));
        }

        [Fact]
        public void HttpsRequiredMatch()
        {
            Assert.True(CheckActionFilterHttp(Constants.TestUri_2, HttpFilterAction.SslRequired));
        }

        [Fact]
        public void HttpsRequiredNoMatch()
        {
            Assert.False(CheckActionFilterHttp(Constants.TestUri_1, HttpFilterAction.SslRequired));
        }

        private bool CheckActionFilterHttp(string input, HttpFilterAction action)
        {
            Uri url = new Uri(input);
            HttpOptionAttribute attribute = new HttpOptionAttribute(action);

            return Evaluate.IsHttpsValid(attribute, url);
        }
    }
}
