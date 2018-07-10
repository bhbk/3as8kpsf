using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class MultipleRegExTests
    {
        [TestMethod]
        public void MultipleDnsAllowRegExMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void MultipleDnsAllowRegExNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void MultipleDnsBlockRegExMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [TestMethod]
        public void MultipleDnsBlockRegExNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyRegEx));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            ActionFilterDnsAddressAttribute attribute =
                new ActionFilterDnsAddressAttribute(new string[] {
                    Statics.TestDns_1_RegEx,
                    Statics.TestDns_3_RegEx,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
