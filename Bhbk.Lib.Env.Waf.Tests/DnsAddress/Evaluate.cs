using Bhbk.Lib.Env.Waf.DnsAddress;
using System.Reflection;

namespace Bhbk.Lib.Env.Waf.Tests.DnsAddress
{
    public class Evaluate
    {
        public static bool IsDnsAddressValid(ActionFilterDnsAddressAttribute attribute, string dns)
        {
            return (bool)typeof(ActionFilterDnsAddressAttribute).GetMethod("IsDnsAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { dns });
        }

        public static bool IsDnsAddressValid(AuthorizeDnsAddressAttribute attribute, string dns)
        {
            return (bool)typeof(AuthorizeDnsAddressAttribute).GetMethod("IsDnsAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { dns });
        }
    }
}
