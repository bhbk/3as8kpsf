using Bhbk.Lib.Waf.Schedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using FakeConstants = Bhbk.Lib.Waf.Tests.Primitives.Constants;
using RealConstants = Bhbk.Lib.Waf.Primitives.Constants;

namespace Bhbk.Lib.Waf.Tests.Schedule
{
    [TestClass]
    public class MultipleOnceTests
    {
        [TestMethod]
        public void MultipleScheduleOnceAllowMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_1, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void MultipleScheduleOnceAllowNoMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_3, ScheduleFilterAction.Allow, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void MultipleScheduleOnceDenyMatch()
        {
            Assert.AreEqual<bool>(false, CheckActionFilterSchedule(FakeConstants.TestWhen_1, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        [TestMethod]
        public void MultipleScheduleOnceDenyNoMatch()
        {
            Assert.AreEqual<bool>(true, CheckActionFilterSchedule(FakeConstants.TestWhen_3, ScheduleFilterAction.Deny, ScheduleFilterOccur.Once));
        }

        private bool CheckActionFilterSchedule(string input, ScheduleFilterAction action, ScheduleFilterOccur occur)
        {
            string padded = string.Empty;

            if (ScheduleHelpers.PadScheduleConfig(input, occur, ref padded))
            {
                DateTime when = DateTime.ParseExact(padded, RealConstants.ApiScheduleFormatUnPadded, null, DateTimeStyles.None);

                ScheduleAttribute attribute =
                    new ScheduleAttribute(new string[] {
                           FakeConstants.TestSchedule_1,
                           FakeConstants.TestSchedule_2
                }, action, occur);

                return Evaluate.IsScheduleValid(attribute, when);
            }
            else
                throw new InvalidOperationException();
        }
    }
}
