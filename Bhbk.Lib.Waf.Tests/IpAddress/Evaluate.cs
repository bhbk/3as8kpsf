using Bhbk.Lib.Waf.IpAddress;
using System.Reflection;

namespace Bhbk.Lib.Waf.Tests.IpAddress
{
    public class Evaluate
    {
        public static bool IsIpAddressValid(ActionFilterIpAddressAttribute attribute, string ip)
        {
            return (bool)typeof(ActionFilterIpAddressAttribute).GetMethod("IsIpAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { ip });
        }
    }
}
