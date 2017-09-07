using Bhbk.Lib.Env.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Env.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleIPv6Tests
    {
        [TestMethod]
        public void MultipleIPv6AllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Allow));
            Assert.AreEqual<bool>(true, CheckAuthorizeIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6AllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv6_3, IpAddressFilterAction.Allow));
            Assert.AreEqual<bool>(false, CheckAuthorizeIpAddress(Statics.TestIPv6_3, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6DenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Deny));
            Assert.AreEqual<bool>(false, CheckAuthorizeIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv6DenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv6_3, IpAddressFilterAction.Deny));
            Assert.AreEqual<bool>(true, CheckAuthorizeIpAddress(Statics.TestIPv6_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute =
                new ActionFilterIpAddressAttribute(new string[] {
                           Statics.TestIPv6_1, 
                           Statics.TestIPv6_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }

        private bool CheckAuthorizeIpAddress(string input, IpAddressFilterAction action)
        {
            AuthorizeIpAddressAttribute attribute =
                new AuthorizeIpAddressAttribute(new string[] {
                           Statics.TestIPv6_1, 
                           Statics.TestIPv6_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
