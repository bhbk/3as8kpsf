using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class SingleTests
    {
        [TestMethod]
        public void SingleDnsAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleDnsAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleDnsBlockMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.Deny));
        }

        [TestMethod]
        public void SingleDnsBlockNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.Deny));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(FakeConstants.TestDns_1, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
