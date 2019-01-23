using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using PillReminder.Model;


namespace PillReminder
{
    public class ReminderManager
    {
        List<PillSchedule> PillsSchedules;

        public event EventHandler<PillReminderEventArg> OnRemindPill;
        List<Tuple<Pill,Time>> currentPills;

        Time checkingInterval;
        IPillReminderIO pillReminderIO;
        ITimeProvider timeProvider;
        ITimer pillReminderCheckingTimer;


        public ReminderManager(Time CheckingInterval,IPillReminderIO PillReminderIO,ITimeProvider TimeProvider,ITimer TimerProvider)
        {
            PillsSchedules = new List<PillSchedule>();
            currentPills = new List<Tuple<Pill, Time>>();

            setDependency(CheckingInterval, PillReminderIO, TimeProvider,TimerProvider);

            intializeReminderTimerData();

        }

        private void setDependency(Time CheckingInterval, IPillReminderIO PillReminderIO, ITimeProvider TimeProvider,ITimer timer)
        {
            checkingInterval = CheckingInterval;
            pillReminderIO = PillReminderIO;
            timeProvider = TimeProvider;
            pillReminderCheckingTimer = timer;
        }

        private void intializeReminderTimerData()
        {
            PillsSchedules = new List<PillSchedule>();
            PillsSchedules = pillReminderIO.GetAllPillSchedule();

            pillReminderCheckingTimer.Interval = checkingInterval.Ticks;
            pillReminderCheckingTimer.AutoReset = true;
            pillReminderCheckingTimer.Enabled = true;
            pillReminderCheckingTimer.Elapsed += checkingNextPileToTake;
            pillReminderCheckingTimer.Start(); 
            
        }

       

        void checkingNextPileToTake(object sender, ElapsedEventArgs eventArgs)
        {
            

            List<Tuple<Pill, Time>> newPillsToTake = new List<Tuple<Pill, Time>>();
            
            foreach (var pill in PillsSchedules)
            {
                Time takingTime;
                if (pill.IsTimeToTake(timeProvider.CurrentTime,out takingTime,checkingInterval))
                {
                    bool havePillTaken = pill.TakenRecordForTheDay.Find(p => p.Item1.Equals(takingTime)).Item2;
                    if (!havePillTaken)
                    {
                        newPillsToTake.Add(new Tuple<Pill, Time>(pill.Pill, takingTime));
                    }
                    
                }
            }

            currentPills.AddRange(newPillsToTake);
            //Remove duplicated
            currentPills = currentPills.Distinct().ToList();

            if (newPillsToTake.Count > 0)
            {
                OnRemindPill?.Invoke(this, new PillReminderEventArg() { PillToTakeWithTime = newPillsToTake });
            }
                     
        }
        public void CheckOffPillTaken(Tuple<Pill,Time> pillTaken)
        {
            int pillScheduleIndex = PillsSchedules.FindIndex(p => p.Pill.Equals(pillTaken.Item1));

            if (pillScheduleIndex != -1)
            {
                int takenRecordForTheDayIndex = PillsSchedules[pillScheduleIndex].TakenRecordForTheDay.FindIndex(p => p.Item2.Equals(pillTaken.Item2));

                if (takenRecordForTheDayIndex != -1)
                {

                    PillsSchedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex]
                        = new Tuple<Time, bool>(PillsSchedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex].Item1, true);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pill given does not exist");
            }


        }

        

    }
}
