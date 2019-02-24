using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;
using Reminder.Model;
using NSubstitute;
using Reminder;
using System.Timers;
using System.Runtime.Serialization;
using System.Reflection;
using System.Linq;
using Reminder.PillReminnder.Model;
using Reminder.PillReminnder;

namespace PillReminderTest.Manager
{
    public class ReminderManagerTest
    {
        ITaskReminderIO<Pill,PillSchedule> pillReminderIOMock;
        ITimeProvider timeProvider;
        ITimer timerMock;
        List<PillSchedule> testPillSchedules;
        List<Pill> testPills;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            testPills = new List<Pill>() { new Pill("Foo", 3), new Pill("Bar", 4), new Pill("Baz", 3),new Pill("RedPill",3) };
            testPillSchedules = new List<PillSchedule>()
            {
                new PillSchedule(testPills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPills[1],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPills[2],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPills[3],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(23,56,00),true),
                }),

            };
           

            pillReminderIOMock = Substitute.For<ITaskReminderIO<Pill,PillSchedule>>();
            pillReminderIOMock.GetAllTaskSchedule().Returns(testPillSchedules);
            pillReminderIOMock.GetAllTask().Returns(testPills);

            Time time = new Time(0, 30, 0);
        }

        [Test]
        public void CheckingPillsToTakeAt_12H_Test()
        {
            // Test Set ups
            testDependencyIntialize(new DateTime(2019, 1, 23, 12, 0, 0));
            

            List<Tuple<Pill, Time>> recivedPillsToTakke = new List<Tuple<Pill, Time>>();
            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(testPills[0],new Time(12,0,0)),
                new Tuple<Pill, Time>(testPills[2],new Time(12,0,0))
            }.OrderBy(p => p.Item1.Name).ToList();
            
            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);
            reminderManager.OnNewTaskReminder += (o, s) => recivedPillsToTakke = s.PillToTakeWithTime;


            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this,createElapsedEventArgs(new Time(12,0,0).ToDateTime()));
       
            Assert.That(recivedPillsToTakke.OrderBy(p => p.Item1.Name).ToList().SequenceEqual(exceptedPills));
        }
        [Test]
        public void CheckingPillsToTakeAt_10H_Test()
        {
            testDependencyIntialize(new DateTime(2019, 1, 23, 10, 0, 0));         

            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>();

            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);
            reminderManager.OnNewTaskReminder += (o, s) => exceptedPills = s.PillToTakeWithTime;

            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(10, 0, 0).ToDateTime()));

            Assert.That(exceptedPills.Count == 0);
        }

        [Test]
        public void CheckingOffPillTakkenAt_18H_Test()
        {
            testDependencyIntialize(new DateTime(2019, 1, 23, 18, 0, 0));
            
            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(18, 0, 0).ToDateTime()));
            reminderManager.CheckingOffFinishedTask(new Tuple<Pill, Time>(testPills[2], new Time(18, 0, 0)));

            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(testPills[0],new Time(18,0,0)),
                new Tuple<Pill, Time>(testPills[1],new Time(18,0,0)),
                
            }.OrderBy(p => p.Item1.Name).ToList();
            List<Tuple<Pill, Time>> acturalPills = new List<Tuple<Pill, Time>>();
            acturalPills = reminderManager.CurrentTask.OrderBy(p => p.Item1.Name).ToList();

            Assert.That(exceptedPills.SequenceEqual(acturalPills));
          
        }
        [Test]
        public void CheckingOffPillOffAt_8H_UpdateCalledTest()
        {
            ITaskReminderIO<Pill, PillSchedule> taskReminderIOMock = Substitute.For<ITaskReminderIO<Pill, PillSchedule>>();
            taskReminderIOMock.GetAllTask().Returns(testPills);
            taskReminderIOMock.GetAllTaskSchedule().Returns(testPillSchedules);

            Time mockedTime = new Time(8, 0, 0);
            timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.CurrentTime.Returns(mockedTime);
            timeProvider.CurrrentDateTime.Returns(mockedTime.ToDateTime());

            timerMock = Substitute.For<ITimer>();

            Tuple<Pill, Time> pillToCheckOff = new Tuple<Pill, Time>(new Pill("Bar", 4,testPills[1].ID), new Time(8, 0, 0));
            PillSchedule exceptedRecivedPillSchedule = new PillSchedule(testPillSchedules[1].ID, testPills[1], new List<Tuple<Time, bool>>()
            {
                    new Tuple<Time, bool>(new Time(8,0,0),true),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
            });
            

            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), taskReminderIOMock, timeProvider, timerMock);
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(mockedTime.ToDateTime()));
            reminderManager.CheckingOffFinishedTask(pillToCheckOff);

            taskReminderIOMock.Received().UpdateTaskScheduleData(Arg.Is<PillSchedule>(p => p.Pill.Equals(pillToCheckOff.Item1)));
        }
        [Test]
        public void CheckingOffPillThatDoesNotExistInSchedules()
        {
            testDependencyIntialize(new DateTime(2019, 1, 23, 22, 0, 0));
            
            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(22, 0, 00).ToDateTime()));
            
            var exceptionRecived = Assert.Throws<ArgumentOutOfRangeException>(()=> reminderManager.CheckingOffFinishedTask(new Tuple<Pill, Time>(new Pill("Red Pill", 3), new Time(22, 0, 0))));

            Assert.That(exceptionRecived.Message, Is.EqualTo("Pill given does not exist\r\nParameter name: Pill"));      
        
        }
        [Test]
        public void CheckingOffPillThatTimeDoesNotExist()
        {
            testDependencyIntialize(new DateTime(2019, 1, 23, 22, 0, 0));
            

            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(22, 0, 0).ToDateTime()));


            var exceptionRecived = Assert.Throws<ArgumentOutOfRangeException>(() => reminderManager.CheckingOffFinishedTask(new Tuple<Pill, Time>(testPills[0], new Time(22, 0, 0))));
            Assert.That(exceptionRecived.Message, Is.EqualTo("Given pilltaken time does not exist in schedule\r\nParameter name: Time"));

        }

      
        [Test]
        public void NoDuplicatedPillTestOnMutippleRemindEvent()
        {
            testDependencyIntialize(new Time(8, 0, 0).ToDateTime());

          
            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);

            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(8, 0, 0).ToDateTime()));
            timeProvider.CurrentTime.Returns(new Time(18, 0, 0));
            timeProvider.CurrrentDateTime.Returns(new Time(18, 0, 0).ToDateTime());
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(18, 0, 0).ToDateTime()));

            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(testPills[1],new Time(8,0,0)),
                new Tuple<Pill, Time>(testPills[0],new Time(18,0,0)),
                new Tuple<Pill, Time>(testPills[1],new Time(18,0,0)),
                new Tuple<Pill, Time>(testPills[2],new Time(18,0,0)),
            }.OrderBy(p => p.Item1.Name).ToList();

            List<Tuple<Pill, Time>> acturalPill = new List<Tuple<Pill, Time>>(reminderManager.CurrentTask).OrderBy(p => p.Item1.Name).ToList();
           

            Assert.That(exceptedPills.SequenceEqual(acturalPill));
        }
        [Test]
        public void MidNightPillNotResetAtMidNightTest()
        {
            testDependencyIntialize(new Time(23, 57, 00).ToDateTime());

            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMock);

            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(23, 57, 00).ToDateTime()));

            List<PillSchedule> exceptedPillSchedulesState = new List<PillSchedule>()
            {
                new PillSchedule(testPillSchedules[0].ID,testPills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[1].ID,testPills[1],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[2].ID,testPills[2],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[3].ID,testPills[3],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(23,56,00),false),
                }),
            };
            List<PillSchedule> currentPillSchedulesState = reminderManager.Schedules;

            Assert.That(currentPillSchedulesState.SequenceEqual(exceptedPillSchedulesState));
        }
        [Test]
        public void PillManagerSavingTest()
        {
            ITaskReminderIO<Pill, PillSchedule> taskReminderIOMock = Substitute.For<ITaskReminderIO<Pill, PillSchedule>>();
            taskReminderIOMock.GetAllTask().Returns(testPills);
            taskReminderIOMock.GetAllTaskSchedule().Returns(testPillSchedules);

            timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.CurrentTime.Returns(new Time(12, 0, 0));
            timeProvider.CurrrentDateTime.Returns(new Time(12, 0, 0).ToDateTime());

            List<Tuple<Pill, Time>> pillToCheckOff = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(testPills[0], new Time(12, 0, 0)),
                new Tuple<Pill, Time>(testPills[2], new Time(12, 0, 0))
            };
            List<PillSchedule> exceptedUpdatePillSchedule = new List<PillSchedule>()
            {
                new PillSchedule(testPillSchedules[0].ID,testPills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),true),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[1].ID,testPills[1],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[2].ID,testPills[2],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),true),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(testPillSchedules[3].ID,testPills[3],new List<Tuple<Time,bool>>()
                {
                    new Tuple<Time, bool>(new Time(23,56,00),true),
                }),
            };

            timerMock = Substitute.For<ITimer>();
          

            PillReminderManager pillReminderManager = new PillReminderManager(new Time(0, 10, 0), taskReminderIOMock, timeProvider, timerMock);
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(12, 0, 0).ToDateTime()));
            pillReminderManager.CheckingOffFinishedTask(pillToCheckOff[0]);
            pillReminderManager.CheckingOffFinishedTask(pillToCheckOff[1]);

            timeProvider.CurrentTime.Returns(new Time(12, 10, 0));
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(12, 10, 0).ToDateTime()));

            timeProvider.CurrentTime.Returns(new Time(12, 20, 0));
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(12, 20, 0).ToDateTime()));


            taskReminderIOMock.Received().UpdateTaskScheduleDatas(Arg.Is<List<PillSchedule>>(p => p[0].TakenRecordForTheDay[1].Item2 == true &&
                                                                                                  p[2].TakenRecordForTheDay[1].Item2 == true));

            timeProvider.CurrentTime.Returns(new Time(12, 30, 0));
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(new Time(12, 30, 0).ToDateTime()));



        }

        void testDependencyIntialize(DateTime mockTime)
        {
            timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.CurrentTime.Returns(new Time(mockTime));
            timeProvider.CurrrentDateTime.Returns(mockTime);
            timerMock = Substitute.For<ITimer>();
        }

        ElapsedEventArgs createElapsedEventArgs(DateTime signalTime)
        {
            var e = FormatterServices.GetUninitializedObject(typeof(ElapsedEventArgs)) as ElapsedEventArgs;
            if (e != null)
            {
                var fieldInfo = e.GetType().GetField("signalTime", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(e, signalTime);
                }
            }

            return e;
        }

    }
}
