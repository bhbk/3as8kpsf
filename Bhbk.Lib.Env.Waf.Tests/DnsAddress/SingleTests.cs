using Bhbk.Lib.Env.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Env.Waf.Tests.DnsAddress
{
    [TestClass]
    public class SingleTests
    {
        [TestMethod]
        public void SingleDnsAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.Allow));
            Assert.AreEqual<bool>(true, CheckAuthorizeDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleDnsAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.Allow));
            Assert.AreEqual<bool>(false, CheckAuthorizeDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.Allow));
        }

        [TestMethod]
        public void SingleDnsBlockMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.Deny));
            Assert.AreEqual<bool>(false, CheckAuthorizeDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.Deny));
        }

        [TestMethod]
        public void SingleDnsBlockNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.Deny));
            Assert.AreEqual<bool>(true, CheckAuthorizeDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.Deny));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            ActionFilterDnsAddressAttribute attribute = new ActionFilterDnsAddressAttribute(Statics.TestDns_1, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }

        private bool CheckAuthorizeDnsAddress(string input, DnsAddressFilterAction action)
        {
            AuthorizeDnsAddressAttribute attribute = new AuthorizeDnsAddressAttribute(Statics.TestDns_1, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
