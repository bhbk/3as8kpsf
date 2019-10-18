using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class SingleDailyTests
    {
        [Fact]
        public void SingleScheduleDailyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_1_Hour, ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily));
        }

        [Fact]
        public void SingleScheduleDailyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_3_Hour, ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily));
        }

        [Fact]
        public void SingleScheduleDailyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(FakeConstants.TestWhen_1_Hour, ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily));
        }

        [Fact]
        public void SingleScheduleDailyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(FakeConstants.TestWhen_3_Hour, ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);
                ScheduleAttribute attribute = new ScheduleAttribute(FakeConstants.TestSchedule_1_Hours, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
