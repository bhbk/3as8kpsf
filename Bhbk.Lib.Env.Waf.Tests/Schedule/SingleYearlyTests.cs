using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class SingleYearlyTests
    {
        [TestMethod]
        public void SingleScheduleYearlyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void SingleScheduleYearlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void SingleScheduleYearlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void SingleScheduleYearlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3_Day, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(occur, input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatDay, null, DateTimeStyles.None);
                ActionFilterScheduleAttribute attribute = new ActionFilterScheduleAttribute(Statics.TestSchedule_1_Days, action, occur);

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
                AuthorizeScheduleAttribute attribute = new AuthorizeScheduleAttribute(Statics.TestSchedule_1_Days, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
