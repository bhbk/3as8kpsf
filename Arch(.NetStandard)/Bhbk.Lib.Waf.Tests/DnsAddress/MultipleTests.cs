using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class MultipleTests
    {
        [Fact]
        public void MultipleDnsAllowMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleDnsAllowNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleDnsBlockMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleDnsBlockNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.Deny));
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
