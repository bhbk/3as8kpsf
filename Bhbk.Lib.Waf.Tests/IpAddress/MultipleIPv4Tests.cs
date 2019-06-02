using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleIPv4Tests
    {
        [TestMethod]
        public void MultipleIPv4AllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4AllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv4_3, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4DenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv4DenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv4_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute =
                new IpAddressAttribute(new string[] {
                           FakeConstants.TestIPv4_1,
                           FakeConstants.TestIPv4_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
