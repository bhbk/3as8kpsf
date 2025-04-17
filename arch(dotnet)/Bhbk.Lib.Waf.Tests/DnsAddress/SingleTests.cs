using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class SingleTests
    {
        [Fact]
        public void SingleDnsAllowMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleDnsAllowNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleDnsBlockMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleDnsBlockNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.Deny));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(Constants.TestDns_1, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
