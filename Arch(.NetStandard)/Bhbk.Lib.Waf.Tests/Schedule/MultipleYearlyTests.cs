using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class MultipleYearlyTests
    {
        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleYearlyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           FakeConstants.TestSchedule_1_DaysOfMonth,
                           FakeConstants.TestSchedule_2_DaysOfMonth
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
