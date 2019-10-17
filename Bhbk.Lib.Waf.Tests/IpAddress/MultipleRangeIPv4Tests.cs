using Bhbk.Lib.Waf.IpAddress;
using System.Net;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class MultipleRangeIPv4Tests
    {
        [Fact]
        public void MultipleIPv4AllowRangeMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv4AllowRangeNoMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Allow));
        }

        [Fact]
        public void MultipleIPv4DenyRangeMatch()
        {
            Assert.False(CheckActionFilterIpAddress(FakeConstants.TestIPv4_1, IpAddressFilterAction.Deny));
        }

        [Fact]
        public void MultipleIPv4DenyRangeNoMatch()
        {
            Assert.True(CheckActionFilterIpAddress(FakeConstants.TestIPv4_2, IpAddressFilterAction.Deny));
        }

        private bool CheckActionFilterIpAddress(string input, IpAddressFilterAction action)
        {
            IpAddressAttribute attribute = new IpAddressAttribute(
                new IPNetwork[] {
                IPNetwork.Parse(FakeConstants.TestIPv4_1_Range),
                IPNetwork.Parse(FakeConstants.TestIPv4_3_Range),
                }, action);
            
            return Evaluate.IsIpAddressValid(attribute, input);
        }
    }
}
