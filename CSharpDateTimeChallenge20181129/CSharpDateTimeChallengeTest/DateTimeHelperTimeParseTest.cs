using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using CSharpDateTimeChallenge;

namespace CSharpDateTimeChallengeTest
{
    [TestFixture]
    public class DateTimeHelperTimeParseTest
    {
        Random random;
        int hour, minute;
        DateTime excepted;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            random = new Random();
        }
        [SetUp]
        public void GenerateMockTime()
        {
            DateTime today = DateTime.Now;

            hour = random.Next(0, 24);
            minute = random.Next(0, 60);

            excepted = new DateTime(today.Year, today.Month, today.Day, hour, minute, 0);
           
        }
        [Test]
        public void ParseTimeInFormtOf_HH_mm()
        {
            string time = $"{hour}:{minute}";
            string formatString = "HH:mm";

            DateTime actural = DateTimeHelper.ParseTime(formatString, time);
            Assert.That(actural == excepted);
        }
        [Test]
        public void ParseTimeInFormatOf_mm_HH()
        {
            string time = $"{minute}:{hour}";
            string formatString = "mm:HH";

            DateTime actural = DateTimeHelper.ParseTime(formatString, time);

            Assert.That(actural == excepted);
        }

    }
}
