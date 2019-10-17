using Bhbk.Lib.Waf.IpAddress;
using System.Net;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class SingleRangeIPv4Tests
    {
        [Fact]
        public void SingleIPv4AllowRangeMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv4AllowRangeNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void SingleIPv4DenyRangeMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void SingleIPv4DenyRangeNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(new IPNetwork[] { IPNetwork.Parse(FakeConstants.TestIPv4_1_Range), }, action);

            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
