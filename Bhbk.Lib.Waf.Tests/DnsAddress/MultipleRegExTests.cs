using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class MultipleRegExTests
    {
        [Fact]
        public void MultipleDnsAllowRegExMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [Fact]
        public void MultipleDnsAllowRegExNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [Fact]
        public void MultipleDnsBlockRegExMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [Fact]
        public void MultipleDnsBlockRegExNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyRegEx));
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
