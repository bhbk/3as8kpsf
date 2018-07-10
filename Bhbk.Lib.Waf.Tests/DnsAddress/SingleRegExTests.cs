using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class SingleRegExTests
    {
        [TestMethod]
        public void SingleDnsAllowRegExMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void SingleDnsAllowRegExNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void SingleDnsBlockRegExMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [TestMethod]
        public void SingleDnsBlockRegExNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyRegEx));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            ActionFilterDnsAddressAttribute attribute = new ActionFilterDnsAddressAttribute(Statics.TestDns_1_RegEx, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
