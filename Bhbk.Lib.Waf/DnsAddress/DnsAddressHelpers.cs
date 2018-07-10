using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Bhbk.Lib.Waf.DnsAddress
{
    public enum DnsAddressFilterAction
    {
        Allow,
        AllowContains,
        AllowRegEx,
        Deny,
        DenyContains,
        DenyRegEx
    }

    public static class DnsAddressHelpers
    {
        public static bool IsDnsAddressAllowed(ref DnsAddressFilterAction action, 
            ref IEnumerable<string> dnsList,
            ref IEnumerable<IPHostEntry> ipList, 
            ref string request)
        {
            IPHostEntry client = Dns.GetHostEntry(request);

            /*
             * Most FQDN forward lookups resolve to an IP address that will NOT resolve back to the 
             * same FQDN... when a reverse lookup is done against the IP address.
             * 
             * Any FQDN we may receive as input must first have a forward lookup executed, then we compare
             * lists of IP address(es) to find allow/deny matches.
             */

            if (dnsList == null)
                throw new InvalidOperationException();

            else if (action == DnsAddressFilterAction.Allow)
            {
                ipList = dnsList.Select(x => Dns.GetHostEntry(x));

                foreach (IPHostEntry entry in ipList)
                    foreach (IPAddress ip in entry.AddressList)
                        if (client.AddressList.Contains(ip))
                            return true;

                return false;
            }
            else if (action == DnsAddressFilterAction.AllowContains)
            {
                ipList = dnsList.Select(x => Dns.GetHostEntry(x));

                foreach (IPHostEntry entry in ipList)
                    if (client.HostName.Contains(entry.HostName))
                        return true;

                return false;
            }
            else if (action == DnsAddressFilterAction.AllowRegEx)
            {
                foreach (string regex in dnsList)
                    if (Regex.IsMatch(client.HostName, regex))
                        return true;
                    
                return false;
            }
            else if (action == DnsAddressFilterAction.Deny)
            {
                ipList = dnsList.Select(x => Dns.GetHostEntry(x));

                foreach (IPHostEntry entry in ipList)
                    foreach (IPAddress ip in entry.AddressList)
                        if (client.AddressList.Contains(ip))
                            return false;

                return true;
            }
            else if (action == DnsAddressFilterAction.DenyContains)
            {
                ipList = dnsList.Select(x => Dns.GetHostEntry(x));

                foreach (IPHostEntry entry in ipList)
                    if (client.HostName.Contains(entry.HostName))
                        return false;

                return true;
            }
            else if (action == DnsAddressFilterAction.DenyRegEx)
            {
                foreach (string regex in dnsList)
                    if (Regex.IsMatch(client.HostName, regex))
                        return false;

                return true;
            }
            else
                throw new InvalidOperationException();
        }

        public static bool IsDnsConfigValid(ref DnsAddressFilterAction action, 
            ref IEnumerable<string> dnsList)
        {
            if (dnsList == null)
                throw new InvalidOperationException();

            else if (action == DnsAddressFilterAction.Allow
                || action == DnsAddressFilterAction.Deny)
            {
                foreach (string entry in dnsList)
                    if (Uri.CheckHostName(entry) != UriHostNameType.Dns)
                        throw new InvalidOperationException();

                return true;
            }
            else if (action == DnsAddressFilterAction.AllowRegEx
                || action == DnsAddressFilterAction.DenyRegEx)
            {
                foreach (string entry in dnsList)
                    if (!Helpers.IsRegExPatternValid(entry))
                        throw new InvalidOperationException();

                return true;
            }
            else
                return true;
        }
    }
}
