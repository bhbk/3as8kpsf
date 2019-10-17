using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class SingleRegExTests
    {
        [Fact]
        public void SingleDnsAllowRegExMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [Fact]
        public void SingleDnsAllowRegExNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [Fact]
        public void SingleDnsBlockRegExMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [Fact]
        public void SingleDnsBlockRegExNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyRegEx));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(FakeConstants.TestDns_1_RegEx, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
