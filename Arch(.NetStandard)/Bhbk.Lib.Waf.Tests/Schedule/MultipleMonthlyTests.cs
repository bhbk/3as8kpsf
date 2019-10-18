using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class MultipleMonthlyTests
    {
        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleMonthlyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleMonthlyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleMonthlyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
        }

        [Fact(Skip = "NotImplemented")]
        public void MultipleScheduleMonthlyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
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
