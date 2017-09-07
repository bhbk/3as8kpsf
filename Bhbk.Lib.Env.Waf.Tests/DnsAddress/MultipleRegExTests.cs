using Bhbk.Lib.Env.Waf.DnsAddress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bhbk.Lib.Env.Waf.Tests.DnsAddress
{
    [TestClass]
    public class MultipleRegExTests
    {
        [TestMethod]
        public void MultipleDnsAllowRegExMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowRegEx));
            Assert.AreEqual<bool>(true, CheckAuthorizeDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void MultipleDnsAllowRegExNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowRegEx));
            Assert.AreEqual<bool>(false, CheckAuthorizeDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [TestMethod]
        public void MultipleDnsBlockRegExMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyRegEx));
            Assert.AreEqual<bool>(false, CheckAuthorizeDnsAddress(Statics.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [TestMethod]
        public void MultipleDnsBlockRegExNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyRegEx));
            Assert.AreEqual<bool>(true, CheckAuthorizeDnsAddress(Statics.TestDns_2, DnsAddressFilterAction.DenyRegEx));
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

        private bool CheckAuthorizeDnsAddress(string input, DnsAddressFilterAction action)
        {
            AuthorizeDnsAddressAttribute attribute =
                new AuthorizeDnsAddressAttribute(new string[] {
                    Statics.TestDns_1_RegEx,
                    Statics.TestDns_3_RegEx,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
