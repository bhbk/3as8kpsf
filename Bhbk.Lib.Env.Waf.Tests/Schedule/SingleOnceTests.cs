using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class SingleOnceTests
    {
        [TestMethod]
        public void SingleScheduleOnceAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void SingleScheduleOnceAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void SingleScheduleOnceDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void SingleScheduleOnceDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(occur, input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatFull, null, DateTimeStyles.None);
                ActionFilterScheduleAttribute attribute = new ActionFilterScheduleAttribute(Statics.TestSchedule_1, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }

        private bool CheckAuthorizeSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(occur, input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatFull, null, DateTimeStyles.None);
                AuthorizeScheduleAttribute attribute = new AuthorizeScheduleAttribute(Statics.TestSchedule_1, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
