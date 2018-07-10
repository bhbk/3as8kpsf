using Bhbk.Lib.Waf.IpAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    [TestClass]
    public class MultipleIPv4Tests
    {
        [TestMethod]
        public void MultipleIPv4AllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4AllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_3, IpAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleIPv4DenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterIpAddress(Statics.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleIPv4DenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterIpAddress(Statics.TestIPv4_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            ActionFilterIpAddressAttribute attribute =
                new ActionFilterIpAddressAttribute(new string[] {
                           Statics.TestIPv4_1,
                           Statics.TestIPv4_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
