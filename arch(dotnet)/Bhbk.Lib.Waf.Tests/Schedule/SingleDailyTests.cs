using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class SingleDailyTests
    {
        [Fact]
        public void SingleScheduleDailyAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_1_Hour, ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily));
        }

        [Fact]
        public void SingleScheduleDailyAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_3_Hour, ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily));
        }

        [Fact]
        public void SingleScheduleDailyDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_1_Hour, ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily));
        }

        [Fact]
        public void SingleScheduleDailyDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_3_Hour, ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);
                ScheduleAttribute attribute = new ScheduleAttribute(Constants.TestSchedule_1_Hours, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
