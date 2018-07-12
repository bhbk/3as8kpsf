using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class SingleRangeIPv4Tests
    {
        [TestMethod]
        public void SingleIPv4AllowRangeMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv4AllowRangeNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleIPv4DenyRangeMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void SingleIPv4DenyRangeNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute = new ActionFilterIpAddressAttribute(new IPNetwork[] { IPNetwork.Parse(Statics.TestIPv4_1_Range), }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
