using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleRangeIPv6Tests
    {
        [TestMethod]
        public void MultipleIPv6AllowRangeMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6AllowRangeNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6DenyRangeMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv6DenyRangeNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute = new ActionFilterIpAddressAttribute(
                new IPNetwork[] {
                IPNetwork.Parse(Statics.TestIPv6_1_Range),
                IPNetwork.Parse(Statics.TestIPv6_3_Range),
                }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
