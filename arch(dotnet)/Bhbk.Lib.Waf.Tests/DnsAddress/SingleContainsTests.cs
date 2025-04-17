using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class SingleContainsTests
    {
        [Fact]
        public void SingleDnsAllowContainsMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void SingleDnsAllowContainsNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void SingleDnsBlockContainsMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(Constants.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [Fact]
        public void SingleDnsBlockContainsNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(Constants.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(Constants.TestDns_1_Contains, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
