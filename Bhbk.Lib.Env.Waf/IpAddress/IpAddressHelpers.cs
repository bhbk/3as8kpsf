using LukeSkywalker.IPNetwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bhbk.Lib.Env.Waf.IpAddress
{
    public enum IpAddressFilterAction
    {
        Allow,
        AllowRegEx,
        Deny,
        DenyRegEx
    }

    public static class IpAddressHelpers
    {
        public static bool IsIpAddressAllowed(ref IpAddressFilterAction action, 
            ref IEnumerable<IPNetwork> cidrList, 
            ref string request)
        {
            IPNetwork client = IPNetwork.Parse(request);

            if (cidrList == null)
                throw new InvalidOperationException();

            else if (action == IpAddressFilterAction.Allow)
                if (cidrList.Any(x => IPNetwork.Contains(x, client)))
                    return true;
                else
                    return false;

            else if (action == IpAddressFilterAction.AllowRegEx)
                throw new NotImplementedException();

            else if (action == IpAddressFilterAction.Deny)
                if (cidrList.Any(x => IPNetwork.Contains(x, client)))
                    return false;
                else
                    return true;

            else if (action == IpAddressFilterAction.DenyRegEx)
                throw new NotImplementedException();

            else
                throw new InvalidOperationException();
        }

        public static bool IsIpConfigValid(ref IpAddressFilterAction action, 
            ref IEnumerable<IPNetwork> cidrList)
        {
            if (cidrList == null)
                throw new InvalidOperationException();

            else if (action == IpAddressFilterAction.AllowRegEx
                || action == IpAddressFilterAction.DenyRegEx)
                throw new NotImplementedException();

            else
                return true;
        }
    }
}
