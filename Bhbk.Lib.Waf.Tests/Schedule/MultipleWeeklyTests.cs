﻿using Bhbk.Lib.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleWeeklyTests
    {
        [TestMethod]
        public void MultipleScheduleWeeklyAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
        }

        [TestMethod]
        public void MultipleScheduleWeeklyAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfWeek, ScheduleFilterAction.Allow, ScheduleFilterOccur.Weekly));
        }

        [TestMethod]
        public void MultipleScheduleWeeklyDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_1_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
        }

        [TestMethod]
        public void MultipleScheduleWeeklyDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_3_DayOfWeek, ScheduleFilterAction.Deny, ScheduleFilterOccur.Weekly));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           FakeConstants.TestSchedule_1_DaysOfWeek,
                           FakeConstants.TestSchedule_2_DaysOfWeek
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
