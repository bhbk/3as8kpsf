using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class MultipleTests
    {
        [TestMethod]
        public void MultipleDnsAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleDnsAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.Allow));
        }

        [TestMethod]
        public void MultipleDnsBlockMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.Deny));
        }

        [TestMethod]
        public void MultipleDnsBlockNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.Deny));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute =
                new DnsAddressAttribute(new string[] {
                    FakeConstants.TestDns_1,
                    FakeConstants.TestDns_3,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
