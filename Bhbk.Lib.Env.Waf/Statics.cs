using System;

namespace Bhbk.Lib.Env.Waf
{
    public static class Statics
    {
        internal const String ApiDnsDynamicAllow = "DnsDynamicAllow";
        internal const String ApiDnsDynamicDeny = "DnsDynamicDeny";
        internal const String ApiDnsDynamicAllowContains = "DnsDynamicAllowContains";
        internal const String ApiDnsDynamicDenyContains = "DnsDynamicDenyContains";
        internal const String ApiDnsDynamicAllowRegEx = "DnsDynamicAllowRegEx";
        internal const String ApiDnsDynamicDenyRegEx = "DnsDynamicDenyRegEx";
        internal const String ApiIpDynamicAllow = "IpDynamicAllow";
        internal const String ApiIpDynamicDeny = "IpDynamicDeny";
        internal const String ApiScheduleDynamicAllow = "ScheduleDynamicAllow";
        internal const String ApiScheduleDynamicDeny = "ScheduleDynamicDeny";
        public const String ApiScheduleConfigFormat = "yyyy:MM:ddTHH:mm:ss";

        internal const String ApiContextIsHttp = "MS_HttpContext";
        internal const String ApiContextIsOwin = "MS_OwinContext";
        internal const String ApiContextIsRemoteEndPoint = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        internal const String MsgApiDnsAddressNotAllowed = "Your DNS address is denied.";
        internal const String MsgApiHttpSessionNotAllowed = "Your HTTP session is denied.";
        internal const String MsgApiIpAddressNotAllowed = "Your IP address is denied.";
        internal const String MsgApiScheduleNotAllowed = "Your schedule is denied.";
    }
}
