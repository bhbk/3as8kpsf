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
                                if (entry.Item1.Month < moment.Month && entry.Item2.Month > moment.Month)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Day < moment.Day && entry.Item2.Day > moment.Day)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Weekly:
                        throw new NotImplementedException();

                    case ScheduleFilterOccur.Daily:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Hour < moment.Hour && entry.Item2.Hour > moment.Hour)
                                    return true;

                            return false;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Minute < moment.Minute && entry.Item2.Minute > moment.Minute)
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
                                if (entry.Item1.Month < moment.Month && entry.Item2.Month > moment.Month)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Monthly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Day < moment.Day && entry.Item2.Day > moment.Day)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Weekly:
                        throw new NotImplementedException();

                    case ScheduleFilterOccur.Daily:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Hour < moment.Hour && entry.Item2.Hour > moment.Hour)
                                    return false;

                            return true;
                        }

                    case ScheduleFilterOccur.Hourly:
                        {
                            foreach (Tuple<DateTime, DateTime> entry in scheduleList)
                                if (entry.Item1.Minute < moment.Minute && entry.Item2.Minute > moment.Minute)
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

                if (DateTime.TryParseExact(dt[0], Statics.ApiScheduleFormatFull, null, DateTimeStyles.None, out begin)
                    && DateTime.TryParseExact(dt[1], Statics.ApiScheduleFormatFull, null, DateTimeStyles.None, out end))
                    schedule.Add(new Tuple<DateTime, DateTime>(begin, end));

                else
                    throw new InvalidOperationException();
            }

            return schedule;
        }
    }
}
