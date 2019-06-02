using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleRangeIPv4Tests
    {
        [TestMethod]
        public void MultipleIPv4AllowRangeMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4AllowRangeNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4DenyRangeMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv4DenyRangeNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(
                new IPNetwork[] {
                IPNetwork.Parse(FakeConstants.TestIPv4_1_Range),
                IPNetwork.Parse(FakeConstants.TestIPv4_3_Range),
                }, action);
            
            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
