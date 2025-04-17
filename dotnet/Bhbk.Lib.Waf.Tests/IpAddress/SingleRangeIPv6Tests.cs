using Bhbk.Lib.Waf.IpAddress;
using Bhbk.Lib.Waf.Tests.Primitives;
using System.Net;
using Xunit;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class SingleRangeIPv6Tests
    {
        [Fact]
        public void SingleIPv6AllowRangeMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6AllowRangeNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv6DenyRangeMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleIPv6DenyRangeNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(new IPNetwork2[] { IPNetwork2.Parse(Constants.TestIPv6_1_Range), }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
