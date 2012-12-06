﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz.TextToSchedule;
using Quartz;
using Quartz.Impl.Calendar;
#if CUSTOM
using Quartz.TextToSchedule.Calendars;
#endif

namespace Quartz.TextToSchedule.Test
{
    [TestClass]
    public class EnglishTests
    {
        public ITextToSchedule tts { get; private set; }

        public EnglishTests()
        {
            tts = new TextToScheduleFactory().CreateEnglishParser();
        }

        [TestMethod]
        public void EverySecond()
        {
            string text = "every second";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();

            TestHelper.AssertHasTimeIntervalOf(trigger, 1, IntervalUnit.Second);
        }

        [TestMethod]
        public void Every5Seconds()
        {
            string text = "every 5 seconds";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();

            TestHelper.AssertHasTimeIntervalOf(trigger, 5, IntervalUnit.Second);
        }

        [TestMethod]
        public void Every30Seconds()
        {
            string text = "every 30 seconds";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();


            TestHelper.AssertHasTimeIntervalOf(trigger, 30, IntervalUnit.Second);
        }

        [TestMethod]
        public void EveryHourOnMonday()
        {
            string text = "every hour on monday";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();


            TestHelper.AssertHasTimeIntervalOf(trigger, 1, IntervalUnit.Hour);

            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Monday);

            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Tuesday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Wednesday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Thursday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Friday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Saturday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Sunday);
        }

        [TestMethod]
        public void EveryHourOnWeekdays()
        {
            string text = "every hour on weekdays";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();

            TestHelper.AssertHasTimeIntervalOf(trigger, 1, IntervalUnit.Hour);

            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Monday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Tuesday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Wednesday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Thursday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Friday);

            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Saturday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Sunday);
        }

        [TestMethod]
        public void EveryHourOnWeekends()
        {
            string text = "every hour on weekends";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();

            TestHelper.AssertHasTimeIntervalOf(trigger, 1, IntervalUnit.Hour);

            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Monday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Tuesday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Wednesday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Thursday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Friday);

            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Saturday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Sunday);
        }

        [TestMethod]
        public void Every2HoursFrom9amTo5pm()
        {
            string text = "every 2 hours from 9 am to 5pm";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();

            TestHelper.AssertHasTimeIntervalOf(trigger, 2, IntervalUnit.Hour);
        }

        [TestMethod]
        public void Every10SecondsOnWeekdaysFrom630To640()
        {
            string text = "every 10 seconds on weekdays from 6:30 to 6:40";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);
            var group = results.RegisterGroups[0];
            var trigger = group.TriggerBuilder.Build();

            TestHelper.AssertHasTimeIntervalOf(trigger, 10, IntervalUnit.Second); 

            TestHelper.AssertDailyCalendarIsTimeIncluded(group, 6, 30, 00);

            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Monday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Tuesday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Wednesday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Thursday);
            TestHelper.AssertWeeklyCalendarHasDayIncluded(group, DayOfWeek.Friday);

            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Saturday);
            TestHelper.AssertWeeklyCalendarHasDayExcluded(group, DayOfWeek.Sunday);
        }

        [TestMethod]
        public void SecondFridayOfJan()
        {
            string text = "2nd fri of jan";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);

            TestHelper.AssertHasCronExpression(results, "0 0 0 ? JAN FRI#2");
        }

        [TestMethod]
        public void EveryMondayOfDecAt4pm()
        {
            string text = "every Monday of DEC at 4pm";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);

            TestHelper.AssertHasCronExpression(results, "0 0 16 ? DEC MON");
        }

        [TestMethod]
        public void LastDayOfMonth()
        {
            string text = "last day of month";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);

            TestHelper.AssertHasCronExpression(results, "0 0 0 L * ?");
        }

        [TestMethod]
        public void FirstDayOfMonth()
        {
            string text = "first day of month";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);

            TestHelper.AssertHasCronExpression(results, "0 0 0 1 * ?");
        }

        [TestMethod]
        public void LastDayOfJanFebDec()
        {
            string text = "last day of January and February and December";
            var results = tts.Parse(text);

            Assert.AreEqual(1, results.RegisterGroups.Count);

            TestHelper.AssertHasCronExpression(results, "0 0 0 L JAN,FEB,DEC ?");
        }

        [TestMethod]
        public void FirstLastMonTueWedOfJanFebDecAt4pm()
        {
            string text = "1st,last mon,tue,wed of jan,feb,dec at 4pm";
            var results = tts.Parse(text);

            Assert.AreEqual(6, results.RegisterGroups.Count);

            TestHelper.AssertHasCronExpression(results, "0 0 16 ? JAN,FEB,DEC MON#1");
            TestHelper.AssertHasCronExpression(results, "0 0 16 ? JAN,FEB,DEC TUE#1");
            TestHelper.AssertHasCronExpression(results, "0 0 16 ? JAN,FEB,DEC WED#1");
            TestHelper.AssertHasCronExpression(results, "0 0 16 ? JAN,FEB,DEC MONL");
            TestHelper.AssertHasCronExpression(results, "0 0 16 ? JAN,FEB,DEC TUEL");
            TestHelper.AssertHasCronExpression(results, "0 0 16 ? JAN,FEB,DEC WEDL");
        }
    }
}