using Bhbk.Lib.Env.Waf.IpAddress;
using System.Reflection;

namespace Bhbk.Lib.Env.Waf.Tests.IpAddress
{
    public class Evaluate
    {
        public static bool IsIpAddressValid(ActionFilterIpAddressAttribute attribute, string ip)
        {
            return (bool)typeof(ActionFilterIpAddressAttribute).GetMethod("IsIpAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { ip });
        }

        public static bool IsIpAddressValid(AuthorizeIpAddressAttribute attribute, string ip)
        {
            return (bool)typeof(AuthorizeIpAddressAttribute).GetMethod("IsIpAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { ip });
        }
    }
}
