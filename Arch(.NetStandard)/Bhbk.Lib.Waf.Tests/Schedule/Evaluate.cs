using Bhbk.Lib.Waf.Schedule;
using System;
using System.Reflection;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class Evaluate
    {
        public static bool IsScheduleValid(ScheduleAttribute attribute, DateTime when)
        {
            return (bool)typeof(ScheduleAttribute).GetMethod("IsScheduleAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { when });
        }
    }
}
