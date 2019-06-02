using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleRangeIPv6Tests
    {
        [TestMethod]
        public void MultipleIPv6AllowRangeMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6AllowRangeNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6DenyRangeMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv6DenyRangeNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(
                new IPNetwork[] {
                IPNetwork.Parse(FakeConstants.TestIPv6_1_Range),
                IPNetwork.Parse(FakeConstants.TestIPv6_3_Range),
                }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
