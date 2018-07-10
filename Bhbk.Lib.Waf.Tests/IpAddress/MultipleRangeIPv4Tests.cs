using Bhbk.Lib.Waf.IpAddress;
using LukeSkywalker.IPNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleRangeIPv4Tests
    {
        [TestMethod]
        public void MultipleIPv4AllowRangeMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4AllowRangeNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4DenyRangeMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv4DenyRangeNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute = new ActionFilterIpAddressAttribute(
                new IPNetwork[] {
                IPNetwork.Parse(Statics.TestIPv4_1_Range),
                IPNetwork.Parse(Statics.TestIPv4_3_Range),
                }, action);
            
            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
