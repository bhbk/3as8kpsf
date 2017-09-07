using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleTests
    {
        [TestMethod]
        public void MultipleScheduleAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void MultipleScheduleAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void MultipleScheduleDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void MultipleScheduleDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleConfigFormat, null, DateTimeStyles.None);

                ActionFilterScheduleAttribute attribute =
                    new ActionFilterScheduleAttribute(new string[] {
                           Statics.TestSchedule_1,
                           Statics.TestSchedule_2
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }

        private bool CheckAuthorizeSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleConfigFormat, null, DateTimeStyles.None);

                AuthorizeScheduleAttribute attribute =
                    new AuthorizeScheduleAttribute(new string[] {
                           Statics.TestSchedule_1,
                           Statics.TestSchedule_2
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
