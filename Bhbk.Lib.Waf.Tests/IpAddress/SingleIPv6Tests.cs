using Bhbk.Lib.Waf.IpAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class SingleIPv6Tests
    {
        [Fact]
        public void SingleIPv6AllowMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6AllowNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6DenyMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleIPv6DenyNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(FakeConstants.TestIPv6_1, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
