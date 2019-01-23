using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;
using PillReminder.Model;
using NSubstitute;
using PillReminder;
using System.Timers;
using System.Runtime.Serialization;
using System.Reflection;
using System.Linq;

namespace PillReminderTest
{
    public class ReminderManaerTest
    {
        IPillReminderIO pillReminderIOMock;
        ITimeProvider timeProvider;
        ITimer timerMoock;
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
           

            pillReminderIOMock = Substitute.For<IPillReminderIO>();
            pillReminderIOMock.GetAllPillSchedule().Returns(pillSchedules);
            pillReminderIOMock.GetAllPill().Returns(pills);

        }

        [SetUp]
        public void SetUp()
        {

        }
        [Test]
        public void CheckingPillsToTakeAt_12AM_Test()
        {
            // Test Set ups
            timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.CurrentTime.Returns(new Time(12, 0, 0));
            timeProvider.CurrrentDateTime.Returns(new DateTime(2019, 1, 23, 12, 0, 0));
            timerMoock = Substitute.For<ITimer>();

            List<Tuple<Pill, Time>> recivedPillsToTakke = new List<Tuple<Pill, Time>>();
            List<Tuple<Pill, Time>> exceptedPills = new List<Tuple<Pill, Time>>()
            {
                new Tuple<Pill, Time>(pills[0],new Time(12,0,0)),
                new Tuple<Pill, Time>(pills[2],new Time(12,0,0))
            }.OrderBy(p => p.Item1.Name).ToList();
            

            ReminderManager reminderManager = new ReminderManager(new Time(0, 5, 0), pillReminderIOMock, timeProvider, timerMoock);
            reminderManager.OnRemindPill += (o, s) => recivedPillsToTakke = s.PillToTakeWithTime;
    
            timerMoock.Elapsed += Raise.Event<ElapsedEventHandler>(this,CreateElapsedEventArgs(new Time(12,5,0).ToDateTime()));

            

            Assert.That(recivedPillsToTakke.OrderBy(p => p.Item1.Name).ToList().SequenceEqual(exceptedPills));
        }

        public ElapsedEventArgs CreateElapsedEventArgs(DateTime signalTime)
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
