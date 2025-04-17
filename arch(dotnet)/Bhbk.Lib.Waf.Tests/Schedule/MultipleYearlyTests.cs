using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class MultipleYearlyTests
    {
        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           Constants.TestSchedule_1_DaysOfMonth,
                           Constants.TestSchedule_2_DaysOfMonth
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
