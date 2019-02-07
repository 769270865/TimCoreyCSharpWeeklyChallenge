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

namespace PillReminderTest
{
    public class ReminderManagerTest
    {
        ITaskReminderIO<Pill,PillSchedule,Guid,Guid> pillReminderIOMock;
        ITimeProvider timeProvider;
        ITimer timerMock;
        List<PillSchedule> pillSchedules;
        List<Pill> pills;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            pills = new List<Pill>() { new Pill("Foo", 3), new Pill("Bar", 4), new Pill("Baz", 3) };
            pillSchedules = new List<PillSchedule>()
            {
                new PillSchedule(pills[0],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(pills[1],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(8,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),
                new PillSchedule(pills[2],new List<Tuple<Time, bool>>()
                {
                    new Tuple<Time, bool>(new Time(6,0,0),false),
                    new Tuple<Time, bool>(new Time(12,0,0),false),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
                }),

            };
           

            pillReminderIOMock = Substitute.For<ITaskReminderIO<Pill,PillSchedule,Guid,Guid>>();
            pillReminderIOMock.GetAllTaskSchedule().Returns(pillSchedules);
            pillReminderIOMock.GetAllTask().Returns(pills);

        }

        [Test]
        public void CheckingPillsToTakeAt_12H_Test()
        {
            // Test Set ups
            testDependencyIntialize(new DateTime(2019, 1, 23, 12, 0, 0));
            

            List<Tuple<Pill, Time>> recivedPillsToTakke = new List<Tuple<Pill, Time>>();
            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(pills[0],new Time(12,0,0)),
                new Tuple<Pill, Time>(pills[2],new Time(12,0,0))
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
            reminderManager.CheckingOffFinishedTask(new Tuple<Pill, Time>(pills[2], new Time(18, 0, 0)));

            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(pills[0],new Time(18,0,0)),
                new Tuple<Pill, Time>(pills[1],new Time(18,0,0)),
                
            }.OrderBy(p => p.Item1.Name).ToList();
            List<Tuple<Pill, Time>> acturalPills = new List<Tuple<Pill, Time>>();
            acturalPills = reminderManager.CurrentTask.OrderBy(p => p.Item1.Name).ToList();

            Assert.That(exceptedPills.SequenceEqual(acturalPills));
          
        }
        [Test]
        public void CheckingOffPillOffAt_8H_UpdateCalledTest()
        {
            ITaskReminderIO<Pill, PillSchedule, Guid, Guid> taskReminderIOMock = Substitute.For<ITaskReminderIO<Pill, PillSchedule, Guid, Guid>>();
            taskReminderIOMock.GetAllTask().Returns(pills);
            taskReminderIOMock.GetAllTaskSchedule().Returns(pillSchedules);

            Time mockedTime = new Time(8, 0, 0);
            timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.CurrentTime.Returns(mockedTime);
            timeProvider.CurrrentDateTime.Returns(mockedTime.ToDateTime());

            timerMock = Substitute.For<ITimer>();

            Tuple<Pill, Time> pillToCheckOff = new Tuple<Pill, Time>(new Pill("Bar", 4,pills[1].ID), new Time(8, 0, 0));
            PillSchedule exceptedRecivedPillSchedule = new PillSchedule(pillSchedules[1].ID, pills[1], new List<Tuple<Time, bool>>()
            {
                    new Tuple<Time, bool>(new Time(8,0,0),true),
                    new Tuple<Time, bool>(new Time(18,0,0),false),
            });
            

            PillReminderManager reminderManager = new PillReminderManager(new Time(0, 5, 0), taskReminderIOMock, timeProvider, timerMock);
            timerMock.Elapsed += Raise.Event<ElapsedEventHandler>(this, createElapsedEventArgs(mockedTime.ToDateTime()));
            reminderManager.CheckingOffFinishedTask(pillToCheckOff);

            taskReminderIOMock.Received().UpdateTaskeScheduleData(Arg.Is<Guid>(p => p == exceptedRecivedPillSchedule.ID),Arg.Is<PillSchedule>(p => p.Pill.Equals(pillToCheckOff.Item1)));
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


            var exceptionRecived = Assert.Throws<ArgumentOutOfRangeException>(() => reminderManager.CheckingOffFinishedTask(new Tuple<Pill, Time>(pills[0], new Time(22, 0, 0))));
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
                new Tuple<Pill, Time>(pills[1],new Time(8,0,0)),
                new Tuple<Pill, Time>(pills[0],new Time(18,0,0)),
                new Tuple<Pill, Time>(pills[1],new Time(18,0,0)),
                new Tuple<Pill, Time>(pills[2],new Time(18,0,0)),
            }.OrderBy(p => p.Item1.Name).ToList();

            List<Tuple<Pill, Time>> acturalPill = new List<Tuple<Pill, Time>>(reminderManager.CurrentTask).OrderBy(p => p.Item1.Name).ToList();
           

            Assert.That(exceptedPills.SequenceEqual(acturalPill));
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
