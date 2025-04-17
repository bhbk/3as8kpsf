using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class MultipleContainsTests
    {
        [Fact]
        public void MultipleDnsAllowContainsMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void MultipleDnsAllowContainsNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void MultipleDnsBlockContainsMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [Fact]
        public void MultipleDnsBlockContainsNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute =
                new DnsAddressAttribute(new string[] {
                    Constants.TestDns_1_Contains,
                    Constants.TestDns_3_Contains,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
