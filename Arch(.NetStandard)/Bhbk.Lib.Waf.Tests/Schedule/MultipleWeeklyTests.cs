using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class MultipleWeeklyTests
    {
        [Fact]
        public void MultipleScheduleWeeklyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
        }

        [Fact]
        public void MultipleScheduleWeeklyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
        }

        [Fact]
        public void MultipleScheduleWeeklyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
        }

        [Fact]
        public void MultipleScheduleWeeklyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           FakeConstants.TestSchedule_1_DaysOfWeek,
                           FakeConstants.TestSchedule_2_DaysOfWeek
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
