using Bhbk.Lib.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleMonthlyTests
    {
        [TestMethod, Ignore]
        public void MultipleScheduleMonthlyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [TestMethod, Ignore]
        public void MultipleScheduleMonthlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [TestMethod, Ignore]
        public void MultipleScheduleMonthlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
        }

        [TestMethod, Ignore]
        public void MultipleScheduleMonthlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
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
