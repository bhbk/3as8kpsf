using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class SingleRegExTests
    {
        [Fact]
        public void SingleDnsAllowRegExMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.AllowRegEx));
        }

        [Fact]
        public void SingleDnsAllowRegExNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.AllowRegEx));
        }

        [Fact]
        public void SingleDnsBlockRegExMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.DenyRegEx));
        }

        [Fact]
        public void SingleDnsBlockRegExNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.DenyRegEx));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(Constants.TestDns_1_RegEx, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
