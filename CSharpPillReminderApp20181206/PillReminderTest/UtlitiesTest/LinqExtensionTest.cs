using System;
using System.Collections.Generic;
using System.Text;
using PillReminder.Utlitie;
using PillReminder.Model;
using NUnit.Framework;

namespace PillReminderTest.UtlitiesTest
{
    public class LinqExtensionTest
    {
        List<PillSchedule> pillSchedules;


        List<Tuple<Time,bool>> threeTimesPerDaySchedule = new List<Tuple<Time, bool>>()
        {
            new Tuple<Time, bool>(new Time(6,0,0),false),
            new Tuple<Time, bool>(new Time(12,0,0),false),
            new Tuple<Time, bool>(new Time(18,0,0),false),
        };


   

    }
}
