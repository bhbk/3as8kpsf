using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class SingleYearlyTests
    {
        [Fact(Skip = "NotImplemented")]
        public void SingleScheduleYearlyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void SingleScheduleYearlyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void SingleScheduleYearlyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void SingleScheduleYearlyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);
                ScheduleAttribute attribute = new ScheduleAttribute(Constants.TestSchedule_1_DaysOfMonth, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
