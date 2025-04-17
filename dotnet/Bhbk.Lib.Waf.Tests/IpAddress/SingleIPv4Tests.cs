using Bhbk.Lib.Waf.IpAddress;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class SingleIPv4Tests
    {
        [Fact]
        public void SingleIPv4AllowMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv4AllowNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv4DenyMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleIPv4DenyNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(Constants.TestIPv4_1, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
