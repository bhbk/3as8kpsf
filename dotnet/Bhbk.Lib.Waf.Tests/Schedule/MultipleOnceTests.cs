using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class MultipleOnceTests
    {
        [Fact]
        public void MultipleScheduleOnceAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [Fact]
        public void MultipleScheduleOnceAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [Fact]
        public void MultipleScheduleOnceDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        [Fact]
        public void MultipleScheduleOnceDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           Constants.TestSchedule_1,
                           Constants.TestSchedule_2
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
