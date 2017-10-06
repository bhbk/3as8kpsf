﻿using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class SingleMonthlyTests
    {
        [TestMethod]
        public void SingleScheduleMonthlyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [TestMethod]
        public void SingleScheduleMonthlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Monthly));
        }

        [TestMethod]
        public void SingleScheduleMonthlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
        }

        [TestMethod]
        public void SingleScheduleMonthlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Monthly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (Bhbk.Lib.Env.Waf.Schedule.ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);
                ActionFilterScheduleAttribute attribute = new ActionFilterScheduleAttribute(Statics.TestSchedule_1_DaysOfMonth, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }

        private bool CheckAuthorizeSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (Bhbk.Lib.Env.Waf.Schedule.ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);
                AuthorizeScheduleAttribute attribute = new AuthorizeScheduleAttribute(Statics.TestSchedule_1_DaysOfMonth, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
