using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleMonthlyTests
    {
        [TestMethod]
        public void MultipleScheduleMonthlyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [TestMethod]
        public void MultipleScheduleMonthlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [TestMethod]
        public void MultipleScheduleMonthlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
        }

        [TestMethod]
        public void MultipleScheduleMonthlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
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
