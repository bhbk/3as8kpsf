using Bhbk.Lib.Waf.IpAddress;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class MultipleIPv4Tests
    {
        [Fact]
        public void MultipleIPv4AllowMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv4AllowNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv4_3, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv4DenyMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleIPv4DenyNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv4_3, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute =
                new IpAddressAttribute(new string[] {
                           FakeConstants.TestIPv4_1,
                           FakeConstants.TestIPv4_2
            }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
