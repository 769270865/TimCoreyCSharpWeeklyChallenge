using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PillReminder;
using PillReminder.Model;

namespace PillReminderTest.Model
{
    public class PillScheduleTest
    {

        List<Tuple<Time, bool>> threeTimesPerDaySchedule;
        PillSchedule testPillSchedule;
        Pill testPill;


        [SetUp]
        public void GenerateTestPillSchedules()
        {

            threeTimesPerDaySchedule = new List<Tuple<Time, bool>>()
            {
                new Tuple<Time, bool>(new Time(6,0,0),false),
                new Tuple<Time, bool>(new Time(12,0,0),false),
                new Tuple<Time, bool>(new Time(18,0,0),false),
            };
            testPill = new Pill("Foo", 3);
            testPillSchedule = new PillSchedule(testPill, threeTimesPerDaySchedule);
           
        }

        [Test]
        public void PillScheduleCheck_IsTimeToTakeMinus5MinutesTest()
        {
            Time currentMocktime = new Time(11, 55, 1);
            Time pillTakingTime = new Time();

            bool isTimeToTakePill = testPillSchedule.IsTimeToTake(currentMocktime, out pillTakingTime);

            Assert.That(isTimeToTakePill && pillTakingTime.Hour == 12 && pillTakingTime.Minute == 0);
        }
        [Test]
        public void PillScheduleCheck_IsTImeToTakePlus5MinutesTest()
        {
            Time currentMocktime = new Time(12, 04, 59);
            Time pillTakingTime = new Time();

            bool isTimeToTakePill = testPillSchedule.IsTimeToTake(currentMocktime, out pillTakingTime);

            Assert.That(isTimeToTakePill && pillTakingTime.Hour == 12 && pillTakingTime.Minute == 0);
        }
        [Test]
        public void PillScheduleDifferentPillNotEqualTest()
        {
            PillSchedule pillScheduleWithDifferentPill = new PillSchedule(new Pill("Bar", 2), threeTimesPerDaySchedule);
            Assert.IsFalse(testPillSchedule.Equals(pillScheduleWithDifferentPill));
        }
        [Test]
        public void PillScheduleDifferentScheduleNotEqualTest()
        {
            List<Tuple<Time, bool>> differentSchedule = new List<Tuple<Time, bool>>()
            {
                new Tuple<Time, bool>( new Time(9,0,0),false),
                new Tuple<Time, bool>(new Time(15,0,0),false),
                new Tuple<Time, bool>(new Time(21,0,0),false),
            };
            PillSchedule pillScheduleWithDifferentSchedule = new PillSchedule(testPill, differentSchedule);

            Assert.IsFalse(testPillSchedule.Equals(pillScheduleWithDifferentSchedule));

        }
        [Test]
        public void PillScheduleDifferentScheduleTimeCountNotEqalTest()
        {
            List<Tuple<Time, bool>> differentSchedule = new List<Tuple<Time, bool>>()
            {
                new Tuple<Time, bool>(new Time(6,0,0),false),
                new Tuple<Time, bool>(new Time(12,0,0),false),
                new Tuple<Time, bool>(new Time(18,0,0),false),
                new Tuple<Time, bool>(new Time(23,0,0),false),
            };
            PillSchedule pillScheduleWithDifferentTimeCouunt = new PillSchedule(new Pill("Foo", 3), differentSchedule);

            Assert.IsFalse(testPillSchedule.Equals(pillScheduleWithDifferentTimeCouunt));

        }
        [Test]
        public void PillScheduleEqualTest()
        {
            PillSchedule secondSchedule = new PillSchedule(new Pill("Foo", 3), new List<Tuple<Time, bool>>()
                                                                                                            {
                                                                                                                new Tuple<Time, bool>(new Time(6,0,0),false),
                                                                                                                new Tuple<Time, bool>(new Time(12,0,0),false),
                                                                                                                new Tuple<Time, bool>(new Time(18,0,0),false),
                                                                                                            });
            Assert.That(secondSchedule.Equals(testPillSchedule));
        }
        [Test]
        public void PillSchedule_ResetTest()
        {
            for (int i = 0; i < testPillSchedule.TakenRecordForTheDay.Count; i++)
            {
                testPillSchedule.TakenRecordForTheDay[i] = new Tuple<Time, bool>(testPillSchedule.TakenRecordForTheDay[i].Item1, true);
            }
            testPillSchedule.ResetSchedule();

            Assert.That(testPillSchedule.TakenRecordForTheDay.FindAll(p => p.Item2).Count == 0);


        }
    }
}