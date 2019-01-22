using NUnit.Framework;
using PillReminder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillReminderTest.Model
{
    public class PillTest
    {
        Pill testPill = new Pill("Foo", 3);

        [Test]
        public void PillEqualTest()
        {
            Pill otherPill = new Pill("Foo", 3);
            Assert.That(testPill.Equals(otherPill));
        }
        [Test]
        public void PillNotEqualTest()
        {
            Pill otherDifferentPill = new Pill("Bar", 2);
            Assert.IsFalse(testPill.Equals(otherDifferentPill));
        }

    }
}
