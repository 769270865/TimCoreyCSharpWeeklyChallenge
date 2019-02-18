using NUnit.Framework;
using Reminder.Model;
using Reminder.PillReminnder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillReminderTest.Model
{
    public class PillTest
    {
        Guid testPillID = new Guid("6795c18a-1807-409f-bae8-fc69bd239a93");
        Pill testPill;

        [SetUp]
        public void SetUp()
        {
            testPill = new Pill("Foo", 3, testPillID);
        }


        [Test]
        public void PillEqualTest()
        {
            
            Pill otherPill = new Pill("Foo", 3,testPillID);
            Assert.That(testPill.Equals(otherPill));
        }
        [Test]
        public void PillNotEqualTest()
        {
            Pill otherDifferentPill = new Pill("Bar", 2,new Guid("918352e9-fe28-4e08-aa2c-4ad8f59a549c"));
            Assert.IsFalse(testPill.Equals(otherDifferentPill));
        }

    }
}
