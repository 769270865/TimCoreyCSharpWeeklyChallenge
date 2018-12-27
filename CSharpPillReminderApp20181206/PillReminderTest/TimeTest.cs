using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;
using PillReminder.Model;

namespace PillReminderTest.Model
{
    public class TimeTest
    {
        DateTime sampleDateTime;
        Time sampleTime;
        double timeOfDayTicks;
        

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sampleDateTime = new DateTime(2018, 12, 24, 14, 15, 10);
            sampleTime = new Time(sampleDateTime);

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
            DateTime testDateTime = sampleTime.ToDateTime();

            Assert.That(testDateTime.Hour == sampleDateTime.Hour && testDateTime.Minute == sampleDateTime.Minute);
                        
                     
        }

    }
}
