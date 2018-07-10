using Bhbk.Lib.Waf.HttpOption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Bhbk.Lib.Waf.Tests.HttpOption
{
    [TestClass]
    public class HttpsTests
    {
        [TestMethod]
        public void HttpsNotAllowedMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterHttp(Statics.TestUri_1, HttpFilterAction.SslNotAllowed));
        }

        [TestMethod]
        public void HttpsNotAllowedNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterHttp(Statics.TestUri_2, HttpFilterAction.SslNotAllowed));
        }

        [TestMethod]
        public void HttpsOptionalMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterHttp(Statics.TestUri_1, HttpFilterAction.SslOptional));
        }

        [TestMethod]
        public void HttpsOptionalNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterHttp(Statics.TestUri_3, HttpFilterAction.SslOptional));
        }

        [TestMethod]
        public void HttpsRequiredMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterHttp(Statics.TestUri_2, HttpFilterAction.SslRequired));
        }

        [TestMethod]
        public void HttpsRequiredNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterHttp(Statics.TestUri_1, HttpFilterAction.SslRequired));
        }

        private bool CheckActionFilterHttp(string input, HttpFilterAction action)
        {
            Uri url = new Uri(input);
            ActionFilterHttpOptionAttribute attribute = new ActionFilterHttpOptionAttribute(action);

            return Evaluate.IsHttpsValid(attribute, url);
        }
    }
}
