using Bhbk.Lib.Waf.IpAddress;
using Bhbk.Lib.Waf.Tests.Primitives;
using System.Net;
using Xunit;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class MultipleRangeIPv6Tests
    {
        [Fact]
        public void MultipleIPv6AllowRangeMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv6AllowRangeNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv6DenyRangeMatch()
        {
            Assert.False(CheckActionFilterIpAddress(Constants.TestIPv6_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleIPv6DenyRangeNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(Constants.TestIPv6_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(
                new IPNetwork2[] {
                IPNetwork2.Parse(Constants.TestIPv6_1_Range),
                IPNetwork2.Parse(Constants.TestIPv6_3_Range),
                }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
