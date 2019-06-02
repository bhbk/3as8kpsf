using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class MultipleContainsTests
    {
        [TestMethod]
        public void MultipleDnsAllowContainsMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void MultipleDnsAllowContainsNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void MultipleDnsBlockContainsMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [TestMethod]
        public void MultipleDnsBlockContainsNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute =
                new DnsAddressAttribute(new string[] {
                    FakeConstants.TestDns_1_Contains,
                    FakeConstants.TestDns_3_Contains,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
