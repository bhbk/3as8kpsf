﻿using Bhbk.Lib.Env.Waf.Schedule;
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
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void SingleScheduleYearlyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Allow, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void SingleScheduleYearlyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
        }

        [TestMethod]
        public void SingleScheduleYearlyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3_DayOfMonth, ScheduleFilterAction.Deny, ScheduleFilterOccur.Yearly));
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
