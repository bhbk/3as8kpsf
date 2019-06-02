using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class SingleIPv4Tests
    {
        [TestMethod]
        public void SingleIPv4AllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv4AllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv4DenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void SingleIPv4DenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(FakeConstants.TestIPv4_1, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
