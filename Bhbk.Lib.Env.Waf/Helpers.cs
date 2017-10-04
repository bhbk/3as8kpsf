using Bhbk.Lib.Env.Waf.Schedule;
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

        public static bool IsDateTimeFormatValid(ScheduleFilterOccur occur, string expr)
        {
            if (!string.IsNullOrEmpty(expr))
            {
                DateTime now;

                switch (occur)
                {
                    case ScheduleFilterOccur.Yearly:
                        {
                            if (DateTime.TryParseExact(expr, Statics.ApiScheduleFormatMonth, null, DateTimeStyles.None, out now))
                                return true;
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            if (DateTime.TryParseExact(expr, Statics.ApiScheduleFormatDay, null, DateTimeStyles.None, out now))
                                return true;
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Weekly:
                        {
                            if (DateTime.TryParseExact(expr, Statics.ApiScheduleFormatDayOfWeek, null, DateTimeStyles.None, out now))
                                return true;
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Daily:
                        {
                            if (DateTime.TryParseExact(expr, Statics.ApiScheduleFormatHour, null, DateTimeStyles.None, out now))
                                return true;
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            if (DateTime.TryParseExact(expr, Statics.ApiScheduleFormatMinute, null, DateTimeStyles.None, out now))
                                return true;
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Once:
                        {
                            if (DateTime.TryParseExact(expr, Statics.ApiScheduleFormatFull, null, DateTimeStyles.None, out now))
                                return true;
                            else
                                return false;
                        }

                    default:
                        throw new InvalidOperationException();
                }
            }
            else
                return false;
        }
    }
}
