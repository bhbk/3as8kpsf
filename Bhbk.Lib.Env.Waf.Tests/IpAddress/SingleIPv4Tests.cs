using Bhbk.Lib.Env.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Env.Waf.Tests.IpAddress
{
    [TestClass]
    public class SingleIPv4Tests
    {
        [TestMethod]
        public void SingleIPv4AllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Allow));
            Assert.AreEqual<bool>(true, CheckAuthorizeIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv4AllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Allow));
            Assert.AreEqual<bool>(false, CheckAuthorizeIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv4DenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Deny));
            Assert.AreEqual<bool>(false, CheckAuthorizeIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void SingleIPv4DenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Deny));
            Assert.AreEqual<bool>(true, CheckAuthorizeIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute = new ActionFilterIpAddressAttribute(Statics.TestIPv4_1, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }

        private bool CheckAuthorizeIpAddress(string input, IpAddressFilterAction action)
        {
            AuthorizeIpAddressAttribute attribute = new AuthorizeIpAddressAttribute(Statics.TestIPv4_1, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
