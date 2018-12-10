using NUnit.Framework;
using CSharpDateTimeChallenge;
using System;

namespace CSharpDateTimeChallengeTests
{
   


    [TestFixture]
    public class DateTimeHelperDateParseTest
    {
        Random random;
        DateTime currentDateTime;
        int year, month, day;
        DateTime excepted;



        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            random = new Random();
            currentDateTime = DateTime.Now;
        }
        [SetUp]
        public void GenerateMockDate()
        {
            year = random.Next(2000, 2021);
            month = random.Next(1, 13);
            day = random.Next(1, 26);
            excepted = new DateTime(year, month, day);
        }
      
        [Test]
        public void ParseDateInFormatOf_YY_MM_DD()
        {
            string formatString = "yy/mm/dd";
            string dateString = $"{year}/{month}/{day}";


           
            DateTime actural = DateTimeHelper.ParseDate(dateString, formatString);

            Assert.That(excepted == actural);
            
        }
        [Test]
        public void ParseDateInFormatOf_YY_DD_MM()
        {
            string formatString = "yy/dd/mm";
            string dateString = $"{year}/{day}/{month}";

            
            DateTime actural = DateTimeHelper.ParseDate(dateString, formatString);

            Assert.That(excepted == actural);
        }
        [Test]
        public void ParseDateInFormatOf_MM_DD_YY()
        {
            string formatString = "mm/dd/yy";
            string dateString = $"{month}/{day}/{year}";

            DateTime actural = DateTimeHelper.ParseDate(dateString, formatString);

            Assert.That(excepted == actural);
        }
        [Test]
        public void ParseDateInFormatOf_MM_YY_DD()
        {
            string formatString = "mm/yy/dd";
            string dateString = $"{month}/{year}/{day}";


            DateTime actural = DateTimeHelper.ParseDate(dateString, formatString);

            Assert.That(excepted == actural);
        }
        [Test]
        public void ParseDateInFormatOf_DD_MM_YY()
        {
            string formatString = "dd/mm/yy";
            string dateString = $"{day}/{month}/{year}";

            DateTime actural = DateTimeHelper.ParseDate(dateString, formatString);

            Assert.That(excepted == actural);
        }
        [Test]
        public void ParseDateInFormatOf_DD_YY_MM()
        {
            string formatString = "dd/yy/mm";
            string dateString = $"{day}/{year}/{month}";

            DateTime actural = DateTimeHelper.ParseDate(dateString, formatString);

            Assert.That(excepted == actural);
        }
    }
}