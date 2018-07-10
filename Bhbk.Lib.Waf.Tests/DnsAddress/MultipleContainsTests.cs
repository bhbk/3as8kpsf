using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class MultipleContainsTests
    {
        [TestMethod]
        public void MultipleDnsAllowContainsMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void MultipleDnsAllowContainsNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void MultipleDnsBlockContainsMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [TestMethod]
        public void MultipleDnsBlockContainsNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            ActionFilterDnsAddressAttribute attribute =
                new ActionFilterDnsAddressAttribute(new string[] {
                    Statics.TestDns_1_Contains,
                    Statics.TestDns_3_Contains,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
