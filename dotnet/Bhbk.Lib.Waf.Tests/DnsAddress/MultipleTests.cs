using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class MultipleTests
    {
        [Fact]
        public void MultipleDnsAllowMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleDnsAllowNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleDnsBlockMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleDnsBlockNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.Deny));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute =
                new DnsAddressAttribute(new string[] {
                    Constants.TestDns_1,
                    Constants.TestDns_3,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
