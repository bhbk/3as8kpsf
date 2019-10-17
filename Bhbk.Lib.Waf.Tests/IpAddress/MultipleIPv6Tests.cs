using Bhbk.Lib.Waf.IpAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class MultipleIPv6Tests
    {
        [Fact]
        public void MultipleIPv6AllowMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv6AllowNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv6_3, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv6DenyMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleIPv6DenyNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv6_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute =
                new IpAddressAttribute(new string[] {
                           FakeConstants.TestIPv6_1, 
                           FakeConstants.TestIPv6_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
