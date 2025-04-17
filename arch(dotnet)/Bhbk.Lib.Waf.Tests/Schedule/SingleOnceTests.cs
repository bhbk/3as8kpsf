using Bhbk.Lib.Waf.Schedule;
using System;
using System.Globalization;
using Xunit;
using Bhbk.Lib.Waf.Tests.Primitives;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    public class SingleOnceTests
    {
        [Fact]
        public void SingleScheduleOnceAllowMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [Fact]
        public void SingleScheduleOnceAllowNoMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [Fact]
        public void SingleScheduleOnceDenyMatch()
        {
            Assert.False(CheckActionFilterSchedule(Constants.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        [Fact]
        public void SingleScheduleOnceDenyNoMatch()
        {
            Assert.True(CheckActionFilterSchedule(Constants.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);
                ScheduleAttribute attribute = new ScheduleAttribute(Constants.TestSchedule_1, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
