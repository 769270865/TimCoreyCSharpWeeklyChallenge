using NUnit.Framework;
using Reminder.Model;
using Reminder.PillReminnder.Model;
using Reminder.PillReminnder.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReminderTest.PillReminderTest.IO
{
    [SetUpFixture]
    public class PillReminderIOTestSetUpFixture
    {
        public string ApplicationBasePath { get; private set; }
        public string TestDataPath { get; private set; }
        public string PillScheduleDataFolderPath { get; private set; }
        public string PillDataFolderPath { get; private set; }
        public PillReminderIOJson PillReminderIO { get; private set; }

        public List<Pill> TestPills { get; private set; }
        public List<PillSchedule> TestPillSchedules { get; set; }

        [OneTimeSetUp]
        public void SetUps()
        {
            setUpTestPillData();
            setUpFilesDirectory();
            PillReminderIO = new PillReminderIOJson(TestDataPath);
        }
        private void setUpTestPillData()
        {

            TestPills = new List<Pill>() { new Pill("Foo", 3), new Pill("Bar", 4), new Pill("Foo", 3) };
            TestPillSchedules = new List<PillSchedule>()
            {
                new PillSchedule(TestPills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(TestPills[1],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                    new Tuple<Time, bool>(new Time(22,0,0),false),
                }),
                new PillSchedule(TestPills[2],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(10,0,0),false),
                    new Tuple<Time, bool>(new Time(16,0,0),false),
                })


            };

           

        }
        private void setUpFilesDirectory()
        {
            ApplicationBasePath = AppDomain.CurrentDomain.BaseDirectory;
            TestDataPath = Path.Combine(ApplicationBasePath, "PillReminderDataTest");
            PillScheduleDataFolderPath = Path.Combine(TestDataPath, "PillScheduleData");
            PillDataFolderPath = Path.Combine(TestDataPath, "PillData");
            Directory.CreateDirectory(TestDataPath);

        }
     
        
    }
   
}
