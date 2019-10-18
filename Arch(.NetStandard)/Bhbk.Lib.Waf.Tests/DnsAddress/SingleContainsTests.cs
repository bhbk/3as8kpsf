using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class SingleContainsTests
    {
        [Fact]
        public void SingleDnsAllowContainsMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void SingleDnsAllowContainsNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void SingleDnsBlockContainsMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [Fact]
        public void SingleDnsBlockContainsNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute = new DnsAddressAttribute(FakeConstants.TestDns_1_Contains, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
