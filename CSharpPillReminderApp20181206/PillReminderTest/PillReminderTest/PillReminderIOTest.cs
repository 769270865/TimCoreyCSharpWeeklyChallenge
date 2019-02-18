using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using Reminder;
using Reminder.Model;
using System.Linq;
using Reminder.PillReminnder.Model;
using Reminder.PillReminnder.Persistence;

namespace PillReminderTest
{
    public class PillReminderIOTest
    {
        string applicationBasePath, testDataPath,pillScheduleDataFolderPath, pillDataFolderPath;
        PillReminderIOJson pillReminderIO;

        List<Pill> testPills;
        List<PillSchedule> testPillSchedules;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            setUpTestPillData();
            setUpFilesDirectory();          
            pillReminderIO = new PillReminderIOJson(testDataPath);
        }

        private void setUpTestPillData()
        {
           
            testPills = new List<Pill>() {new Pill("Foo",3),new Pill("Bar",4),new Pill("Foo",3) };
            testPillSchedules = new List<PillSchedule>()
            {
                new PillSchedule(testPills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPills[1],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                    new Tuple<Time, bool>(new Time(22,0,0),false),
                }),
                new PillSchedule(testPills[2],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(10,0,0),false),
                    new Tuple<Time, bool>(new Time(16,0,0),false),
                })

                
            };
                
                
                            
        }
        private void setUpFilesDirectory()
        {
            applicationBasePath = AppDomain.CurrentDomain.BaseDirectory;
            testDataPath = Path.Combine(applicationBasePath, "PillReminderDataTest");
            pillScheduleDataFolderPath = Path.Combine(testDataPath, "PillScheduleData");
            pillDataFolderPath = Path.Combine(testDataPath, "PillData");
            Directory.CreateDirectory(testDataPath);
        }


        [SetUp]
        public void SetUp()
        {
            string[] pillScheduleDataFiles = Directory.GetFiles(pillScheduleDataFolderPath, "*.dat");
            string[] pillDataFiles = Directory.GetFiles(pillDataFolderPath, "*.dat");

            foreach (var filePath in pillScheduleDataFiles)
            {
                File.Delete(filePath);
            }
            foreach (var filePath in pillDataFiles)
            {
                File.Delete(filePath);
            }
        }
       
        [Test]
        public void GetPillTest()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(Path.Combine($"{pillDataFolderPath}", $"{testPills[0].ID.ToString()}.dat")))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, testPills[0]);
            }

            List<Pill> existedPills = pillReminderIO.GetAllTask();

            Assert.That(existedPills.Count == 1 && existedPills[0].Equals(testPills[0]));
        }
        [Test]
        public void GetAllPillTest()
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
                using (StreamWriter sw = new StreamWriter(Path.Combine($"{pillDataFolderPath}", $"{pill.Name}.dat")))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, pill);
                }
            }

            List<Pill> retrivedPill = new List<Pill>();
            retrivedPill = pillReminderIO.GetAllTask().OrderBy(p=> p.Name).ToList();

            Assert.That(testPill.SequenceEqual(retrivedPill)); 

        }        
        [Test]
        public void GetPillSchheduleTest()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{testPillSchedules[0].ID.ToString()}_Schedule.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                PillScheduleStorageObject testPillScheduleStorageObject = new PillScheduleStorageObject(testPillSchedules[0]);
                serializer.Serialize(writer, testPillScheduleStorageObject);
            }
            List<PillSchedule> retrivedPillSchedule = pillReminderIO.GetAllTaskSchedule();
            Assert.That(retrivedPillSchedule.Count == 1 && retrivedPillSchedule[0].Equals(testPillSchedules[0]));
        }
        [Test]
        public void GetAllPillSchedulesTest()
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
            List<PillScheduleStorageObject> testPillSchedulesStorageObject = new List<PillScheduleStorageObject>();
            testPillSchedules.ForEach(p => testPillSchedulesStorageObject.Add((new PillScheduleStorageObject(p))));


            JsonSerializer serializer = new JsonSerializer();
            foreach (var schedule in testPillSchedulesStorageObject)
            {
                using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{schedule.Pill.Name}_Schedule.dat"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, schedule);
                }
            }

            List<PillSchedule> retrivedSchedule = pillReminderIO.GetAllTaskSchedule().OrderBy(p => p.Pill.Name).ToList();

            Assert.That(testPillSchedules.SequenceEqual(retrivedSchedule));
        }

        [Test]
        public void SavePillDataTest()
        {
            pillReminderIO.SaveTaskData(testPills[0]);

            Assert.That(File.Exists(Path.Combine(pillDataFolderPath, $"{testPills[0].ID.ToString()}.dat")));

        }
        [Test]
        public void SaveAllPillDataTest()
        {
            
        }
        [Test]
        public void SavePillScheduleTest()
        {
            pillReminderIO.SaveTaskScheduleData(testPillSchedules[0]);

            Assert.That(File.Exists(Path.Combine(pillScheduleDataFolderPath, $"{testPillSchedules[0].ID.ToString()}_Schedule.dat")));
        }


        [Test]
        public void UpdatePillTest()
        {
            // Save Test Pill
            JsonSerializer seralizer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{testPills[0].ID.ToString()}.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                seralizer.Serialize(writer, testPills);
            }

            // Update Pill
            Pill updatedPill = new Pill("Foo", 4, testPills[0].ID);
            
            pillReminderIO.UpdateTaskData(updatedPill);

            //Retrive udated pill
            Pill acturalPill;
            using (StreamReader sr = new StreamReader($@"{pillDataFolderPath}\{testPills[0].ID.ToString()}.dat"))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                acturalPill = seralizer.Deserialize<Pill>(reader);
            }

            Assert.That(acturalPill.Equals(updatedPill));
        }
        [Test]
        public void UpdatePillsTest()
        {
            JsonSerializer serializer = new JsonSerializer();
            saveTestPills(serializer);

            List<Pill> updatedPill = new List<Pill>()
            {
                new Pill(testPills[0].Name + "_Updated",testPills[0].Quantity,testPills[0].ID),
                new Pill(testPills[1].Name + "_Updated",testPills[1].Quantity + 1,testPills[1].ID),
                new Pill(testPills[2].Name + "_Updated",testPills[2].Quantity - 1,testPills[2].ID),
            }.OrderBy(p => p.ID).ToList();

            pillReminderIO.UpdateTaskDatas(updatedPill);

            List<Pill> acturalUpdatedPills = new List<Pill>();
            for (int i = 0; i < updatedPill.Count; i++)
            {
                using (StreamReader sr = new StreamReader($@"{pillDataFolderPath}\{updatedPill[i].ID.ToString()}.dat"))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    acturalUpdatedPills.Add(serializer.Deserialize<Pill>(reader));
                }
            }
            acturalUpdatedPills.OrderBy(p => p.ID).ToList();
            string[] fileInFilePath = Directory.GetFiles(pillDataFolderPath, "*.dat");

            Assert.That(fileInFilePath.Length == 3 && updatedPill.SequenceEqual(acturalUpdatedPills));

        }

        private void saveTestPills(JsonSerializer serializer)
        {
            foreach (var pill in testPills)
            {
                using (StreamWriter sr = new StreamWriter($@"{pillDataFolderPath}\{pill.ID.ToString()}.dat"))
                using (JsonWriter writer = new JsonTextWriter(sr))
                {
                    serializer.Serialize(writer, pill);
                }
            }
        }

        [Test]
        public void UpdatePillWithNonExistIDTest()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{testPills[0].ID.ToString()}.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, testPills[0]);
            }
            Pill updatePill = new Pill("Foo", 3, Guid.NewGuid());

            var exceptionRecived = Assert.Throws<ArgumentException>(() => pillReminderIO.UpdateTaskData( updatePill));
            Assert.That(exceptionRecived.Message, Is.EqualTo("Specificed file does not exist"));
        }      
        [Test]
        public void UpdatePillSchedule()
        {
            PillSchedule updatedPillSchedule = new PillSchedule(testPillSchedules[0].ID,
                                                                testPills[0], 
                                                                    new List<Tuple<Time, bool>>()                                                             
                                                                    {
                                                                        new Tuple<Time, bool>(new Time(9,0,0),false),
                                                                        new Tuple<Time, bool>(new Time(13,0,0),false),
                                                                        new Tuple<Time, bool>(new Time(20,0,0),false),
                                                                    }
                                                                );
            PillSchedule retrivedPillSchedule;
            PillScheduleStorageObject retrivedPillScheduleInStorageObj;

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{testPillSchedules[0].ID.ToString()}_Schedule.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer,  new PillScheduleStorageObject(testPillSchedules[0]));
            }

            pillReminderIO.UpdateTaskScheduleData(updatedPillSchedule);

            using (StreamReader sr = new StreamReader($@"{pillScheduleDataFolderPath}\{testPillSchedules[0].ID.ToString()}_Schedule.dat"))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                retrivedPillScheduleInStorageObj = serializer.Deserialize<PillScheduleStorageObject>(reader);
            }
            retrivedPillSchedule = retrivedPillScheduleInStorageObj.PillScheduleStorageObjectToPillSchedule();

            Assert.That(updatedPillSchedule.Equals(retrivedPillSchedule));

        }
        [Test]
        public void UpdatePillSchedulesTest()
        {
            JsonSerializer serializer = new JsonSerializer();
            saveTestPillSchedules(serializer);

            List<PillSchedule> updatedPillSchedule = new List<PillSchedule>()
            {
                new PillSchedule(testPillSchedules[0].ID,testPills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[1].ID,testPills[1],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(10,0,0),false),
                    new Tuple<Time, bool>(new Time(14,0,0),false),
                    new Tuple<Time, bool>(new Time(20,0,0),false),
                    
                }),
                new PillSchedule(testPillSchedules[2].ID,testPills[2],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(10,0,0),false),
                    new Tuple<Time, bool>(new Time(16,0,0),false),
                    new Tuple<Time, bool>(new Time(21,0,0),false),
                })
            }.OrderBy(p => p.ID).ToList();
            

            pillReminderIO.UpdateTaskScheduleDatas(updatedPillSchedule);

            List<PillScheduleStorageObject> acturalPillSchedulesStorageObject = new List<PillScheduleStorageObject>();
            List<PillSchedule> acturalPillSchedule = new List<PillSchedule>();
            for (int i = 0; i < updatedPillSchedule.Count; i++)
            {
                using (StreamReader sr = new StreamReader($@"{pillScheduleDataFolderPath}\{updatedPillSchedule[i].ID.ToString()}_Schedule.dat"))
                using (JsonReader jsonReader = new JsonTextReader(sr))
                {
                    acturalPillSchedulesStorageObject.Add(serializer.Deserialize<PillScheduleStorageObject>(jsonReader));
                }
            }
            foreach (var pillScheledStorage in acturalPillSchedulesStorageObject)
            {
                acturalPillSchedule.Add(pillScheledStorage.PillScheduleStorageObjectToPillSchedule());
            }

            Assert.That(acturalPillSchedule.SequenceEqual(updatedPillSchedule));
        }
        private void saveTestPillSchedules(JsonSerializer serializer)
        {
            foreach (var pillSchedule in testPillSchedules)
            {
                PillScheduleStorageObject scheduleStorageObject = new PillScheduleStorageObject(pillSchedule);
                using (StreamWriter sr = new StreamWriter($@"{pillScheduleDataFolderPath}\{pillSchedule.ID.ToString()}_Schedule.dat"))
                using (JsonWriter writer = new JsonTextWriter(sr))
                {
                    serializer.Serialize(writer, scheduleStorageObject);
                }
            }
        }



        [Test]
        public void DeletePillScheduleTest()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{testPillSchedules[0].ID.ToString()}_Schedule.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, new PillScheduleStorageObject(testPillSchedules[0]));
            }

            pillReminderIO.DeleteTaskScheduleData(testPillSchedules[0]);

            Assert.That(!File.Exists($@"{pillScheduleDataFolderPath}\{testPillSchedules[0].ID.ToString()}_Schedule.dat"));
        }
        [Test]
        public void DeletePillTest()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{testPills[0].ID.ToString()}.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, testPills);
            }

            pillReminderIO.DeleteTaskData(testPills[0]);
            Assert.That(!File.Exists($@"{pillDataFolderPath}\{testPills[0].ID.ToString()}.dat"));

        }
       
    }
}
