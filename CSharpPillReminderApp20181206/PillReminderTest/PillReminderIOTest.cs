using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using PillReminder;
using PillReminder.Model;
using System.Linq;
using PillReminder.Persistence;

namespace PillReminderTest
{
    public class PillReminderIOTest
    {
        string applicationBasePath, testDataPath,pillScheduleDataFolderPath, pillDataFolderPath;
        PillReminderIO pillReminderIO;

        Pill testPill;
        PillSchedule testPillSchedule;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            setUpFilesDirectory();

            setUpTestPillData();
        }

        private void setUpTestPillData()
        {
            testPill = new Pill("Foo", 3);
            testPillSchedule = new PillSchedule(testPill, new List<Tuple<Time, bool>>()
                                                                                     { new Tuple<Time, bool>(new Time(6,0,0),false),
                                                                                       new Tuple<Time, bool>(new Time(12,0,0),false),
                                                                                       new Tuple<Time, bool>(new Time(18,0,0),false),
                                                                                     });
        }

        private void setUpFilesDirectory()
        {
            applicationBasePath = AppDomain.CurrentDomain.BaseDirectory;
            testDataPath = Path.Combine(applicationBasePath, "PillReminderDataTest");
            pillScheduleDataFolderPath = Path.Combine(testDataPath, "PillScheduleData");
            pillDataFolderPath = Path.Combine(testDataPath, "PillData");
        }

        [SetUp]
        public void SetUp()
        {
            if (Directory.Exists(pillScheduleDataFolderPath))
            {
                Directory.Delete(pillScheduleDataFolderPath, true);
            }

            if (Directory.Exists(pillDataFolderPath))
            {
                Directory.Delete(pillDataFolderPath, true);
            }
            pillReminderIO = new PillReminderIO(testDataPath);

        }
        [Test]
        public void SavePillDataTest()
        {
            pillReminderIO.SavePillData(testPill);

            Assert.That(File.Exists(Path.Combine(pillDataFolderPath, $"{testPill.Name}.txt")));

        }
        [Test]
        public void GetPillDataTest()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(Path.Combine($"{pillDataFolderPath}", $"{testPill.Name}.txt")))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, testPill);
            }

            List<Pill> existedPills = pillReminderIO.GetAllPill();

            Assert.That(existedPills.Count == 1 && existedPills[0].Equals(testPill));
        }
        [Test]
        public void GetAllPillDataTest()
        {
          
            List<Pill> testPill = new List<Pill>()
            {
                new Pill("Foo",3),
                new Pill("Bar",4),
                new Pill("Baz",3),
            }.OrderBy(p => p.Name).ToList();


            JsonSerializer serializer = new JsonSerializer();
            foreach (var pill in testPill)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine($"{pillDataFolderPath}", $"{pill.Name}.txt")))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, pill);
                }
            }

            List<Pill> retrivedPill = new List<Pill>();
            retrivedPill = pillReminderIO.GetAllPill().OrderBy(p=> p.Name).ToList();

            Assert.That(testPill.SequenceEqual(retrivedPill)); 

        }

        [Test]
        public void SavePillScheduleTest()
        {
            pillReminderIO.SavePillSchedule(testPillSchedule);

            Assert.That(File.Exists(Path.Combine(pillScheduleDataFolderPath, $"{testPillSchedule.Pill.Name}_Schedule.txt")));
        }

        [Test]
        public void GetPillSchedulesTest()
        {
            List<PillSchedule> testPillSchedules = new List<PillSchedule>()
            {
                new PillSchedule(new Pill("Foo",3),new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(new Pill("Bar",2),new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(20,0,0),false),
                }),
                new PillSchedule(new Pill("Baz",3),new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                    new Tuple<Time, bool>(new Time(22,0,0),false),
                }),
                
            }.OrderBy(p => p.Pill.Name).ToList();

            JsonSerializer serializer = new JsonSerializer();
            foreach (var schedule in testPillSchedules)
            {
                using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{schedule.Pill.Name}_Schedule.txt"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, schedule);
                }
            }

            List<PillSchedule> retrivedSchedule = pillReminderIO.GetAllPillSchedule().OrderBy(p => p.Pill.Name).ToList();

            Assert.That(testPillSchedules.SequenceEqual(retrivedSchedule));
        }


       
    }
}
