﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Waf.IpAddress
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
                if (cidrList.Any(x => client.Contains(x)))
                    return true;
                else
                    return false;

            else if (action == IpAddressFilterAction.AllowRegEx)
                throw new NotImplementedException();

            else if (action == IpAddressFilterAction.Deny)
                if (cidrList.Any(x => client.Contains(x)))
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

    public class IpAddressException : Exception
    {
        public IpAddressException(string message)
            : base(message) { }

        public IpAddressException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class IpAddressParseException : IpAddressException
    {
        public IpAddressParseException(string message)
            : base(message) { }
    }
}
