using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class SingleContainsTests
    {
        [TestMethod]
        public void SingleDnsAllowContainsMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void SingleDnsAllowContainsNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void SingleDnsBlockContainsMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [TestMethod]
        public void SingleDnsBlockContainsNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(FakeConstants.TestDns_1_Contains, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
