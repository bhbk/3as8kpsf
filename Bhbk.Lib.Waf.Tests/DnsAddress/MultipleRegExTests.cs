using Bhbk.Lib.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    [TestClass]
    public class MultipleRegExTests
    {
        [TestMethod]
        public void MultipleDnsAllowRegExMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void MultipleDnsAllowRegExNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void MultipleDnsBlockRegExMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [TestMethod]
        public void MultipleDnsBlockRegExNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyRegEx));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute =
                new DnsAddressAttribute(new string[] {
                    FakeConstants.TestDns_1_RegEx,
                    FakeConstants.TestDns_3_RegEx,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
