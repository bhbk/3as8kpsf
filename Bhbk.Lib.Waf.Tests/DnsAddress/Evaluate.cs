using Bhbk.Lib.Waf.DnsAddress;
using System.Reflection;

namespace Bhbk.Lib.Waf.Tests.DnsAddress
{
    public class Evaluate
    {
        public static bool IsDnsAddressValid(DnsAddressAttribute attribute, string dns)
        {
            return (bool)typeof(DnsAddressAttribute).GetMethod("IsDnsAddressAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { dns });
        }
    }
}
