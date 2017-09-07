using Bhbk.Lib.Env.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Env.Waf.Tests.DnsAddress
{
    [TestClass]
    public class SingleContainsTests
    {
        [TestMethod]
        public void SingleDnsAllowContainsMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowContains));
            Assert.AreEqual<bool>(true, CheckAuthorizeDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void SingleDnsAllowContainsNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowContains));
            Assert.AreEqual<bool>(false, CheckAuthorizeDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [TestMethod]
        public void SingleDnsBlockContainsMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyContains));
            Assert.AreEqual<bool>(false, CheckAuthorizeDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [TestMethod]
        public void SingleDnsBlockContainsNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyContains));
            Assert.AreEqual<bool>(true, CheckAuthorizeDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            ActionFilterDnsAddressAttribute attribute = new ActionFilterDnsAddressAttribute(Statics.TestDns_1_Contains, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }

        private bool CheckAuthorizeDnsAddress(string input, DnsAddressFilterAction action)
        {
            AuthorizeDnsAddressAttribute attribute = new AuthorizeDnsAddressAttribute(Statics.TestDns_1_Contains, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
