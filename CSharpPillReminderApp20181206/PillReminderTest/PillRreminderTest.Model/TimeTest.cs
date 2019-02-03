using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;
using Reminder.Model;

namespace PillReminderTest.Model
{
    public class TimeTest
    {
        DateTime sampleDateTime;
        Time testTime;
        double timeOfDayTicks;
        

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sampleDateTime = new DateTime(2018, 12, 24, 14, 15, 10);
            testTime = new Time(sampleDateTime);

            timeOfDayTicks = sampleDateTime.TimeOfDay.Ticks;

            
        }


        [Test]
        public void DateTimeToTimeTest()
        {        
            Time testTime = new Time(sampleDateTime);
          
            Assert.That(testTime.Hour == 14 && testTime.Minute == 15 && testTime.Second == 10 && testTime.Ticks == timeOfDayTicks);
        }
        [Test]
        public void TimeToDateTimeTest()
        {
            DateTime testDateTime = testTime.ToDateTime();

            Assert.That(testDateTime.Hour == sampleDateTime.Hour && testDateTime.Minute == sampleDateTime.Minute);
                        
                     
        }
        [Test]
        public void TimeEqualTest()
        {
            Time time = new Time(sampleDateTime.Hour, sampleDateTime.Minute, sampleDateTime.Second);
            Assert.That(time.Equals(testTime));
        }
        [Test]
        public void TimeNotEqualTest()
        {
            Time differentTime = new Time(23, 59, 59);
            Assert.False(differentTime.Equals(testTime));
        }


    }
}
