using Bhbk.Lib.Waf.IpAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class MultipleIPv6Tests
    {
        [Fact]
        public void MultipleIPv6AllowMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv6AllowNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_3, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv6DenyMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleIPv6DenyNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute =
                new IpAddressAttribute(new string[] {
                           Constants.TestIPv6_1, 
                           Constants.TestIPv6_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
