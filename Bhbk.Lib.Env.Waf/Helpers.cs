using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Bhbk.Lib.Env.Waf
{
    public static class Helpers
    {
        /*
         * Simplest way to test for an invalid regular expression pattern is to try & use it then
         * catch exception if one is thrown.
         */
        public static bool IsRegExPatternValid(string expr)
        {
            if (!string.IsNullOrEmpty(expr))
            {
                try
                {
                    Regex.Match(string.Empty, expr);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public static bool IsDateTimeFormatValid(string expr)
        {
            if (!string.IsNullOrEmpty(expr))
            {
                DateTime now;

                if (DateTime.TryParseExact(expr, Statics.ApiScheduleConfigFormat, null, DateTimeStyles.None, out now))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
