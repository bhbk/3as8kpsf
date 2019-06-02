using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleIPv6Tests
    {
        [TestMethod]
        public void MultipleIPv6AllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6AllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv6_3, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv6DenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv6DenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv6_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute =
                new IpAddressAttribute(new string[] {
                           FakeConstants.TestIPv6_1, 
                           FakeConstants.TestIPv6_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
