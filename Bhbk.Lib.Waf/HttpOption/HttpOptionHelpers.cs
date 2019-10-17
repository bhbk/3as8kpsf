using System;

namespace Bhbk.Lib.Waf.HttpOption
{
    public enum HttpFilterAction
    {
        SslRequired,
        SslNotAllowed,
        SslOptional
    }

    public static class HttpOptionHelpers
    {
        public static bool IsHttpOptionAllowed(ref HttpFilterAction action, ref Uri url)
        {
            if (action == HttpFilterAction.SslNotAllowed
                && url.Scheme == Uri.UriSchemeHttp)
                return true;

            else if (action == HttpFilterAction.SslRequired
                && url.Scheme == Uri.UriSchemeHttps)
                return true;

            else if (action == HttpFilterAction.SslOptional
                && (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps))
                return true;

            else
                return false;
        }
    }

    public class HttpAddressException : Exception
    {
        public HttpAddressException(string message)
            : base(message) { }

        public HttpAddressException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class HttpAddressParseException : HttpAddressException
    {
        public HttpAddressParseException(string message)
            : base(message) { }
    }
}
