using System;

namespace Bhbk.Lib.Env.Waf.Tests
{
    internal static class Statics
    {
        internal static readonly String TestDns_1 = "www.debian.org";
        internal static readonly String TestDns_2 = "www.kernel.org";
        internal static readonly String TestDns_3 = "www.microsoft.com";

        internal static readonly String TestDns_1_Contains = "debian.org";
        internal static readonly String TestDns_2_Contains = "kernel.org";
        internal static readonly String TestDns_3_Contains = "microsoft.com";

        internal static readonly String TestDns_1_RegEx = @"[a-zA-Z0-9]*debian\.[a-z]*$";
        internal static readonly String TestDns_2_RegEx = @"[a-zA-Z0-9]*kernel\.[a-z]*$";
        internal static readonly String TestDns_3_RegEx = @"[a-zA-Z0-9]*microsoft\.[a-z]*$";

        internal static readonly String TestIPv4_1 = "1.1.1.1";
        internal static readonly String TestIPv4_2 = "2.2.2.2";
        internal static readonly String TestIPv4_3 = "3.3.3.3";

        internal static readonly String TestIPv4_1_Range = "1.1.1.0/8";
        internal static readonly String TestIPv4_2_Range = "2.2.2.0/8";
        internal static readonly String TestIPv4_3_Range = "3.3.3.0/8";

        internal static readonly String TestIPv6_1 = "2001:0da1::1";
        internal static readonly String TestIPv6_2 = "2001:0da2::1";
        internal static readonly String TestIPv6_3 = "2001:0da3::1";

        internal static readonly String TestIPv6_1_Range = "2001:0da1::/64";
        internal static readonly String TestIPv6_2_Range = "2001:0da2::/64";
        internal static readonly String TestIPv6_3_Range = "2001:0da3::/64";

        internal static readonly String TestWhen_1 = "1801:01:01T06:00:01";
        internal static readonly String TestWhen_2 = "1901:01:01T06:00:01";
        internal static readonly String TestWhen_3 = "2001:01:01T06:00:01";

        internal static readonly String TestSchedule_1 = "1801:01:01T06:00:00-1899:01:01T06:00:00";
        internal static readonly String TestSchedule_2 = "1901:01:01T06:00:00-1999:01:01T06:00:00";
        internal static readonly String TestSchedule_3 = "2001:01:01T06:00:00-2001:01:01T06:00:00";

        internal static readonly String TestUri_1 = "http://kernel.org";
        internal static readonly String TestUri_2 = "https://kernel.org";
        internal static readonly String TestUri_3 = "ftp://kernel.org";
    }
}
