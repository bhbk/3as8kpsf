﻿using Bhbk.Lib.Env.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Env.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleWeeklyTests
    {
        [TestMethod]
        public void MultipleScheduleWeeklyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_1_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
        }

        [TestMethod]
        public void MultipleScheduleWeeklyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_3_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
        }

        [TestMethod]
        public void MultipleScheduleWeeklyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
            Assert.AreEqual<bool>(false, CheckAuthorizeSchedule(Statics.TestWhen_1_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
        }

        [TestMethod]
        public void MultipleScheduleWeeklyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
            Assert.AreEqual<bool>(true, CheckAuthorizeSchedule(Statics.TestWhen_3_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            if (Bhbk.Lib.Env.Waf.Helpers.IsDateTimeFormatValid(occur, input))
            {
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatDayOfWeek, null, DateTimeStyles.None);

                ActionFilterScheduleAttribute attribute =
                    new ActionFilterScheduleAttribute(new string[] {
                           Statics.TestSchedule_1_DaysOfWeek,
                           Statics.TestSchedule_2_DaysOfWeek
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
                DateTime when = DateTime.ParseExact(input, Bhbk.Lib.Env.Waf.Statics.ApiScheduleFormatDayOfWeek, null, DateTimeStyles.None);

                AuthorizeScheduleAttribute attribute =
                    new AuthorizeScheduleAttribute(new string[] {
                           Statics.TestSchedule_1_DaysOfWeek,
                           Statics.TestSchedule_2_DaysOfWeek
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
