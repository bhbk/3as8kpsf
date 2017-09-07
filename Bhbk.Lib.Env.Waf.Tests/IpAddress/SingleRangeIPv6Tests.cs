using Bhbk.Lib.Env.Waf.IpAddress;
using LukeSkywalker.IPNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Env.Waf.Tests.IpAddress
{
    [TestClass]
    public class SingleRangeIPv6Tests
    {
        [TestMethod]
        public void SingleIPv6AllowRangeMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Allow));
            Assert.AreEqual<bool>(true, CheckAuthorizeIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv6AllowRangeNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv6_2, IpAddressFilterAction.Allow));
            Assert.AreEqual<bool>(false, CheckAuthorizeIpAddress(Statics.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv6DenyRangeMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Deny));
            Assert.AreEqual<bool>(false, CheckAuthorizeIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void SingleIPv6DenyRangeNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv6_2, IpAddressFilterAction.Deny));
            Assert.AreEqual<bool>(true, CheckAuthorizeIpAddress(Statics.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute = new ActionFilterIpAddressAttribute(new IPNetwork[] { IPNetwork.Parse(Statics.TestIPv6_1_Range), }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }

        private bool CheckAuthorizeIpAddress(string input, IpAddressFilterAction action)
        {
            AuthorizeIpAddressAttribute attribute = new AuthorizeIpAddressAttribute(new IPNetwork[] { IPNetwork.Parse(Statics.TestIPv6_1_Range), }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
