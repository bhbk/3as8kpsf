using System;

namespace Bhbk.Lib.Waf.Tests
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

        internal static readonly String TestWhen_1 = "1801:1:1T6:0:1";
        internal static readonly String TestWhen_2 = "1901:1:1T6:0:1";
        internal static readonly String TestWhen_3 = "2001:1:1T6:0:1";
        internal static readonly String TestWhen_1_Minute = "9";
        internal static readonly String TestWhen_2_Minute = "29";
        internal static readonly String TestWhen_3_Minute = "59";
        internal static readonly String TestWhen_1_Hour = "3";
        internal static readonly String TestWhen_2_Hour = "15";
        internal static readonly String TestWhen_3_Hour = "23";
        internal static readonly String TestWhen_1_DayOfWeek = "Tuesday";
        internal static readonly String TestWhen_2_DayOfWeek = "Thursday";
        internal static readonly String TestWhen_3_DayOfWeek = "Saturday";
        internal static readonly String TestWhen_1_DayOfMonth = "3";
        internal static readonly String TestWhen_2_DayOfMonth = "13";
        internal static readonly String TestWhen_3_DayOfMonth = "23";

        internal static readonly String TestSchedule_1 = "1801:1:1T6:0:0-1899:1:1T6:0:0";
        internal static readonly String TestSchedule_2 = "1901:1:1T6:0:0-1999:1:1T6:0:0";
        internal static readonly String TestSchedule_3 = "2001:1:1T6:0:0-2001:1:1T6:0:0";
        internal static readonly String TestSchedule_1_Minutes = "0-19";
        internal static readonly String TestSchedule_2_Minutes = "20-39";
        internal static readonly String TestSchedule_3_Minutes = "40-59";
        internal static readonly String TestSchedule_1_Hours = "0-7";
        internal static readonly String TestSchedule_2_Hours = "8-15";
        internal static readonly String TestSchedule_3_Hours = "16-23";
        internal static readonly String TestSchedule_1_DaysOfWeek = "Monday-Tuesday";
        internal static readonly String TestSchedule_2_DaysOfWeek = "Wednesday-Friday";
        internal static readonly String TestSchedule_3_DaysOfWeek = "Saturday-Sunday";
        internal static readonly String TestSchedule_1_DaysOfMonth = "1-9";
        internal static readonly String TestSchedule_2_DaysOfMonth = "10-18";
        internal static readonly String TestSchedule_3_DaysOfMonth = "19-28";

        internal static readonly String TestUri_1 = "http://kernel.org";
        internal static readonly String TestUri_2 = "https://kernel.org";
        internal static readonly String TestUri_3 = "ftp://kernel.org";
    }
}
