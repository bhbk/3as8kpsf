﻿using Bhbk.Lib.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleDailyTests
    {
        [TestMethod]
        public void MultipleScheduleDailyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_1_Hour, ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily));
        }

        [TestMethod]
        public void MultipleScheduleDailyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_3_Hour, ScheduleFilterAction.Allow, ScheduleFilterOccur.Daily));
        }

        [TestMethod]
        public void MultipleScheduleDailyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(Statics.TestWhen_1_Hour, ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily));
        }

        [TestMethod]
        public void MultipleScheduleDailyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(Statics.TestWhen_3_Hour, ScheduleFilterAction.Deny, ScheduleFilterOccur.Daily));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (Bhbk.Lib.Waf.Schedule.ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, Bhbk.Lib.Waf.Statics.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ActionFilterScheduleAttribute attribute =
                    new ActionFilterScheduleAttribute(new string[] {
                           Statics.TestSchedule_1_Hours,
                           Statics.TestSchedule_2_Hours
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}