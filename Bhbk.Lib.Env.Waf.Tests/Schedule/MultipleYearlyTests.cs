using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleYearlyTests
    {
        [TestMethod]
        public void MultipleScheduleYearlyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void MultipleScheduleYearlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void MultipleScheduleYearlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void MultipleScheduleYearlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(occur, input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatDay, null, DateTimeStyles.None);

                ActionFilterScheduleAttribute attribute =
                    new ActionFilterScheduleAttribute(new string[] {
                           Statics.TestSchedule_1_Days,
                           Statics.TestSchedule_2_Days
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }

        private bool CheckAuthorizeSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(occur, input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatDay, null, DateTimeStyles.None);

                AuthorizeScheduleAttribute attribute =
                    new AuthorizeScheduleAttribute(new string[] {
                           Statics.TestSchedule_1_Days,
                           Statics.TestSchedule_2_Days
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
