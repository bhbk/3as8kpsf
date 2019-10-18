using Bhbk.Lib.Waf.IpAddress;
using System.Net;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class SingleRangeIPv6Tests
    {
        [Fact]
        public void SingleIPv6AllowRangeMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6AllowRangeNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6DenyRangeMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleIPv6DenyRangeNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(new IPNetwork[] { IPNetwork.Parse(FakeConstants.TestIPv6_1_Range), }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
