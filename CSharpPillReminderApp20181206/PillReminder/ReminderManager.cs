using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Reminder.Model;
using Reminder.PillReminnder.Model;

namespace Reminder.PillReminnder
{
    public class ReminderManager : TaskReminderr<Pill,PillSchedule,PillReminderEventArg>
    {
        List<PillSchedule> PillsSchedules;

        public override event EventHandler<PillReminderEventArg> OnNewTaskReminder;
        public override List<Tuple<Pill, Time>> CurrentTask { get; }

        Time checkingInterval;

        ITaskReminderIO<Pill,PillSchedule,Guid,Guid> pillReminderIO;
        ITimeProvider timeProvider;
        ITimer pillReminderCheckingTimer;

        

        public ReminderManager(Time CheckingInterval,ITaskReminderIO<Pill,PillSchedule,Guid,Guid> PillReminderIO,ITimeProvider TimeProvider,ITimer TimerProvider)
        {
            PillsSchedules = new List<PillSchedule>();
            CurrentTask = new List<Tuple<Pill, Time>>();
            checkingInterval = CheckingInterval;

            setDependency(PillReminderIO, TimeProvider,TimerProvider);

            intializeReminderTimerData();

        }

        private void setDependency(ITaskReminderIO<Pill,PillSchedule,Guid,Guid> PillReminderIO, ITimeProvider TimeProvider,ITimer timer)
        {
           
            pillReminderIO = PillReminderIO;
            timeProvider = TimeProvider;
            pillReminderCheckingTimer = timer;
        }

        private void intializeReminderTimerData()
        {
            PillsSchedules = new List<PillSchedule>();
            PillsSchedules = pillReminderIO.GetAllTaskSchedule();

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
                    
                    newPillsToTake.Add(new Tuple<Pill, Time>(pill.Pill, takingTime));
                                       
                }
            }

            CurrentTask.AddRange(newPillsToTake);
           

            if (newPillsToTake.Count > 0)
            {
                OnNewTaskReminder?.Invoke(this, new PillReminderEventArg() { PillToTakeWithTime = newPillsToTake });
            }
                     
        }
        public override void CheckingOffFinishedTask(Tuple<Pill,Time> pillTaken)
        {
            int pillScheduleIndex = PillsSchedules.FindIndex(p => p.Pill.Equals(pillTaken.Item1));

            if (pillScheduleIndex != -1)
            {
                int takenRecordForTheDayIndex = PillsSchedules[pillScheduleIndex].TakenRecordForTheDay.FindIndex(p => p.Item1.Equals(pillTaken.Item2));

                if (takenRecordForTheDayIndex != -1)
                {

                    PillsSchedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex]
                        = new Tuple<Time, bool>(PillsSchedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex].Item1, true);
                    CurrentTask.Remove(pillTaken);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Time","Given pilltaken time does not exist in schedule");
                    
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pill","Pill given does not exist");
            }


        }
      


    }
}
