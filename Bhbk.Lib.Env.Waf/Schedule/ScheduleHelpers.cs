using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Bhbk.Lib.Env.Waf.Schedule
{
    public enum ScheduleFilterAction
    {
        Allow,
        Deny,
    }

    public enum ScheduleFilterOccur
    {
        Once,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public static class ScheduleHelpers
    {
        public static bool IsScheduleAllowed(ref ScheduleFilterAction action,
            ref ScheduleFilterOccur occur,
            ref List<Tuple<DateTime, DateTime>> scheduleList,
            ref DateTime when)
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
                                if (entry.Item1.Year < when.Year && entry.Item2.Year > when.Year)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Month < when.Month && entry.Item2.Month > when.Month)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Weekly:
                        throw new NotImplementedException();

                    case ScheduleFilterOccur.Daily:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Day < when.Day && entry.Item2.Day > when.Day)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Hour < when.Hour && entry.Item2.Hour > when.Hour)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Once:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1 < when && entry.Item2 > when)
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
                                if (entry.Item1.Year < when.Year && entry.Item2.Year > when.Year)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Month < when.Month && entry.Item2.Month > when.Month)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Weekly:
                        throw new NotImplementedException();

                    case ScheduleFilterOccur.Daily:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Day < when.Day && entry.Item2.Day > when.Day)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Hour < when.Hour && entry.Item2.Hour > when.Hour)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Once:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1 < when && entry.Item2 > when)
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
                    case ScheduleFilterOccur.Weekly:
                    case ScheduleFilterOccur.Daily:
                    case ScheduleFilterOccur.Hourly:
                        throw new NotImplementedException();

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

        public static List<Tuple<DateTime, DateTime>> ParseScheduleConfig(IEnumerable<string> config)
        {
            List<Tuple<DateTime, DateTime>> schedule = new List<Tuple<DateTime, DateTime>>();
            DateTime begin, end;

            foreach (string token in config)
            {
                string[] dt = token.Split('-').Select(x => x.Trim()).ToArray();

                if (DateTime.TryParseExact(dt[0], Statics.ApiScheduleConfigFormat, null, DateTimeStyles.None, out begin)
                    && DateTime.TryParseExact(dt[1], Statics.ApiScheduleConfigFormat, null, DateTimeStyles.None, out end))
                    schedule.Add(new Tuple<DateTime, DateTime>(begin, end));

                else
                    throw new InvalidOperationException();
            }

            return schedule;
        }
    }
}
