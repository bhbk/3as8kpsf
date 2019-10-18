using Bhbk.Lib.Waf.DnsAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class MultipleContainsTests
    {
        [Fact]
        public void MultipleDnsAllowContainsMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void MultipleDnsAllowContainsNoMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.AllowContains));
        }

        [Fact]
        public void MultipleDnsBlockContainsMatch()
        {
            Assert.False(CheckActionFilterDnsAddress(FakeConstants.TestDns_1, DnsAddressFilterAction.DenyContains));
        }

        [Fact]
        public void MultipleDnsBlockContainsNoMatch()
        {
            Assert.True(CheckActionFilterDnsAddress(FakeConstants.TestDns_2, DnsAddressFilterAction.DenyContains));
        }

        private bool CheckActionFilterDnsAddress(string input, DnsAddressFilterAction action)
        {
            DnsAddressAttribute attribute =
                new DnsAddressAttribute(new string[] {
                    FakeConstants.TestDns_1_Contains,
                    FakeConstants.TestDns_3_Contains,
                }, action);

            return Evaluate.IsDnsAddressValid(attribute, input);
        }
    }
}
