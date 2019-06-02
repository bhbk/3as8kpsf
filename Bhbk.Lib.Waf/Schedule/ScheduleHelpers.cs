using Bhbk.Lib.Waf.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bhbk.Lib.Waf.Schedule
{
    public enum ScheduleFilterAction
    {
        Allow,
        Deny,
    }

    public enum ScheduleFilterOccur
    {
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Once
    }

    public static class ScheduleHelpers
    {
        public static bool IsScheduleAllowed(ref ScheduleFilterAction action,
            ref ScheduleFilterOccur occur,
            ref List<Tuple<DateTime, DateTime>> scheduleList,
            ref DateTime moment)
        {
            if (scheduleList == null)
                throw new InvalidOperationException();

            else if (action == ScheduleFilterAction.Allow)
            {
                switch (occur)
                {
                    case ScheduleFilterOccur.Yearly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Month <= moment.Month && entry.Item2.Month >= moment.Month)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Day <= moment.Day && entry.Item2.Day >= moment.Day)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Weekly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.DayOfWeek <= moment.DayOfWeek && entry.Item2.DayOfWeek >= moment.DayOfWeek)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Daily:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Hour <= moment.Hour && entry.Item2.Hour >= moment.Hour)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Minute <= moment.Minute && entry.Item2.Minute >= moment.Minute)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Once:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1 < moment && entry.Item2 > moment)
                                    return true;

                            return false;
                        }

                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (action == ScheduleFilterAction.Deny)
            {
                switch (occur)
                {
                    case ScheduleFilterOccur.Yearly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Month <= moment.Month && entry.Item2.Month >= moment.Month)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Day <= moment.Day && entry.Item2.Day >= moment.Day)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Weekly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.DayOfWeek <= moment.DayOfWeek && entry.Item2.DayOfWeek >= moment.DayOfWeek)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Daily:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Hour <= moment.Hour && entry.Item2.Hour >= moment.Hour)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Minute <= moment.Minute && entry.Item2.Minute >= moment.Minute)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Once:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1 < moment && entry.Item2 > moment)
                                    return false;

                            return true;
                        }

                    default:
                        throw new InvalidOperationException();
                }
            }
            else
                throw new InvalidOperationException();
        }

        public static bool IsScheduleConfigValid(ref ScheduleFilterAction action,
            ref ScheduleFilterOccur occur,
            ref List<Tuple<DateTime, DateTime>> scheduleList)
        {
            if (scheduleList == null)
                throw new InvalidOperationException();

            else if (action == ScheduleFilterAction.Allow
                || action == ScheduleFilterAction.Deny)
            {
                switch (occur)
                {
                    case ScheduleFilterOccur.Yearly:
                    case ScheduleFilterOccur.Monthly:
                        throw new NotImplementedException();

                    case ScheduleFilterOccur.Weekly:                    
                    case ScheduleFilterOccur.Daily:                    
                    case ScheduleFilterOccur.Hourly:
                    case ScheduleFilterOccur.Once:
                        foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                            if (entry.Item1 >= entry.Item2)
                                return false;
                        return true;

                    default:
                        throw new InvalidOperationException();
                }
            }
            else
                throw new InvalidOperationException();
        }

        public static List<Tuple<DateTime, DateTime>> ParseScheduleConfig(IEnumerable<string> config, ScheduleFilterOccur occur)
        {
            List<Tuple<DateTime, DateTime>> schedule = new List<Tuple<DateTime, DateTime>>();
            DateTime begin, end;

            foreach (string token in config)
            {
                string[] dt = token.Split('-').Select(x => x.Trim()).ToArray();
                string start = string.Empty, finish = string.Empty;

                if(PadScheduleConfig(dt[0], occur, ref start)
                    && PadScheduleConfig(dt[1], occur, ref finish))

                    if (DateTime.TryParseExact(start, Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out begin)
                        && DateTime.TryParseExact(finish, Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out end))
                        schedule.Add(new Tuple<DateTime, DateTime>(begin, end));

                    else
                        throw new InvalidOperationException();

                else
                    throw new InvalidOperationException();
            }

            return schedule;
        }

        public static bool PadScheduleConfig(string input, ScheduleFilterOccur occur, ref string padded)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DateTime now;

                switch (occur)
                {
                    case ScheduleFilterOccur.Yearly:
                        {
                            if (DateTime.TryParseExact(input, Constants.ApiScheduleFormatMonth, null, DateTimeStyles.None, out now))
                            {
                                padded = now.ToString(Constants.ApiScheduleFormatUnPadded);
                                return true;
                            }
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            if (DateTime.TryParseExact("0001:" + input + ":1T0:0:0 ", Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out now))
                            {
                                padded = now.ToString(Constants.ApiScheduleFormatUnPadded);
                                return true;
                            }
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Weekly:
                        {
                            if(input != DayOfWeek.Sunday.ToString()
                                && input != DayOfWeek.Monday.ToString()
                                && input != DayOfWeek.Tuesday.ToString()
                                && input != DayOfWeek.Wednesday.ToString()
                                && input != DayOfWeek.Thursday.ToString()
                                && input != DayOfWeek.Friday.ToString()
                                && input != DayOfWeek.Saturday.ToString())
                                throw new InvalidOperationException();

                            else if (DateTime.TryParseExact("0001:1:1T0:0:0", Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out now))
                            {
                                while (now.DayOfWeek.ToString() != input)
                                    now = now.AddDays(1);

                                padded = now.ToString(Constants.ApiScheduleFormatUnPadded);
                                return true;
                            }
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Daily:
                        {
                            if (DateTime.TryParseExact("0001:1:1T" + input + ":0:0", Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out now))
                            {
                                padded = now.ToString(Constants.ApiScheduleFormatUnPadded);
                                return true;
                            }
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            if (DateTime.TryParseExact("0001:1:1T0:" + input + ":0", Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out now))
                            {
                                padded = now.ToString(Constants.ApiScheduleFormatUnPadded);
                                return true;
                            }
                            else
                                return false;
                        }

                    case ScheduleFilterOccur.Once:
                        {
                            if (DateTime.TryParseExact(input, Constants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None, out now))
                            {
                                padded = now.ToString(Constants.ApiScheduleFormatUnPadded);
                                return true;
                            }
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

        //private static DateTime FindNextWeekdayOccur(DayOfWeek day)
        //{
        //    DateTime when = DateTime.Now.AddDays(1);

        //    while (when.DayOfWeek != day)
        //        when = when.AddDays(1);

        //    return when;
        //}
    }
}
