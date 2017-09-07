using Bhbk.Lib.Env.Waf.Schedule;
using System;
using System.Reflection;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    public class Evaluate
    {
        public static bool IsScheduleValid(ActionFilterScheduleAttribute attribute, DateTime when)
        {
            return (bool)typeof(ActionFilterScheduleAttribute).GetMethod("IsScheduleAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { when });
        }

        public static bool IsScheduleValid(AuthorizeScheduleAttribute attribute, DateTime when)
        {
            return (bool)typeof(AuthorizeScheduleAttribute).GetMethod("IsScheduleAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { when });
        }
    }
}
