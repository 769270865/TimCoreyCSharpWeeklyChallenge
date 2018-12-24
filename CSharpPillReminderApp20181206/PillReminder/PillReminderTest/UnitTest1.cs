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
        List<PillSchedule> testPillSchedules;
        

       
        [OneTimeSetUp]
        public void GenerateTestPillSchedules()
        {

            List<string> pillNames = new List<string>()
            {
                "Alpha", "Bravo","Charli","Delta","Echol",
                "Froxtra","Golf","Hotel","India","Juliet",
            };

            List<Pill> pills = new List<Pill>()
            {
                new Pill("Alpha",3),new Pill("Bravo",3),new Pill("Charli",2),new Pill("Delta",2),new Pill("Echol",2),
            };

            List<Tuple<Time, bool>> fourTimesPerDaySchedule = new List<Tuple<Time, bool>>()
            {
                new Tuple<Time, bool>(new Time(6,0,0),false),
                new Tuple<Time, bool>(new Time(12,0,0),false),
                new Tuple<Time, bool>(new Time(18,0,0),false),
                new Tuple<Time, bool>(new Time(22,0,0),false)
            };

            List<Tuple<Time, bool>> threeTimesPerDaySchedule = new List<Tuple<Time, bool>>()
            {
                new Tuple<Time, bool>(new Time(6,0,0),false),
                new Tuple<Time, bool>(new Time(12,0,0),false),
                new Tuple<Time, bool>(new Time(18,0,0),false),
            };
            List<Tuple<Time, bool>> twoTimesPerDaySchedule = new List<Tuple<Time, bool>>()
            {
                new Tuple<Time, bool>(new Time(6,0,0),false),
                new Tuple<Time, bool>(new Time(18,0,0),false)
            };

            for (int i = 0; i < 10; i++)
            {
               
            }
        }


        [SetUp]
        public void Setup()
        {


        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}