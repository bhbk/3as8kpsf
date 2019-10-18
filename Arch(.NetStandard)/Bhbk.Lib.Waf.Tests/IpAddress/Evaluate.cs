using Bhbk.Lib.Waf.IpAddress;
using System.Reflection;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class Evaluate
    {
        public static bool IsIpAddressValid(IpAddressAttribute attribute, string ip)
        {
            return (bool)typeof(IpAddressAttribute).GetMethod("IsIpAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { ip });
        }
    }
}
