using Bhbk.Lib.Waf.IpAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class SingleIPv6Tests
    {
        [Fact]
        public void SingleIPv6AllowMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6AllowNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6DenyMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleIPv6DenyNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(Constants.TestIPv6_1, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
