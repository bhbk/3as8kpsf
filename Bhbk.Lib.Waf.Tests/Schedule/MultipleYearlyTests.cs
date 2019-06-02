using Bhbk.Lib.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleYearlyTests
    {
        [TestMethod, Ignore]
        public void MultipleScheduleYearlyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod, Ignore]
        public void MultipleScheduleYearlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod, Ignore]
        public void MultipleScheduleYearlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [TestMethod, Ignore]
        public void MultipleScheduleYearlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
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
