using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class MultipleHourlyTests
    {
        [Fact]
        public void MultipleScheduleHourlyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_1_Minute, ScheduleFilterAction.Allow, ScheduleFilterOccur.Hourly));
        }

        [Fact]
        public void MultipleScheduleHourlyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_3_Minute, ScheduleFilterAction.Allow, ScheduleFilterOccur.Hourly));
        }

        [Fact]
        public void MultipleScheduleHourlyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_1_Minute, ScheduleFilterAction.Deny, ScheduleFilterOccur.Hourly));
        }

        [Fact]
        public void MultipleScheduleHourlyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_3_Minute, ScheduleFilterAction.Deny, ScheduleFilterOccur.Hourly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           FakeConstants.TestSchedule_1_Minutes,
                           FakeConstants.TestSchedule_2_Minutes
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
