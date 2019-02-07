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
    public class PillReminderManager : TaskReminderr<Pill,PillSchedule,PillReminderEventArg>
    {
        List<PillSchedule> PillsSchedules;

        public override event EventHandler<PillReminderEventArg> OnNewTaskReminder;
        public override List<Tuple<Pill, Time>> CurrentTask { get; }

        Time checkingInterval;

        ITaskReminderIO<Pill,PillSchedule,Guid,Guid> pillReminderIO;
        ITimeProvider timeProvider;
        ITimer pillReminderCheckingTimer;

        

        public PillReminderManager(Time CheckingInterval,ITaskReminderIO<Pill,PillSchedule,Guid,Guid> PillReminderIO,ITimeProvider TimeProvider,ITimer TimerProvider)
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
            pillReminderCheckingTimer.Elapsed += onIntervalPassed;
            pillReminderCheckingTimer.Start(); 
            
        }

        void onIntervalPassed(object sender,ElapsedEventArgs eventArgs)
        {
            checkingNextPileToTake(sender, eventArgs);

            resetAllPillAtMidNight(sender,eventArgs);
        }
        void resetAllPillAtMidNight(object _sender,ElapsedEventArgs _eventArgs)
        {   
            if (timeProvider.CurrentTime.Ticks > (Time.MAX_TICKS - checkingInterval.Ticks / 2) && timeProvider.CurrentTime.Ticks < (checkingInterval.Ticks / 2) )
            {
                resetAtMidNight();

            }
            else
            {
                return;
            }
        }

        private void resetAtMidNight()
        {
            IEnumerable<PillSchedule> pillToTakeAtMidNight =
                                                        PillsSchedules.FindAll(p => p.IsTimeToTake(timeProvider.CurrentTime, checkingInterval));
            if (pillToTakeAtMidNight.Count() > 0)
            {
                foreach (var pillSchedule in PillsSchedules)
                {
                    if (!pillToTakeAtMidNight.Contains(pillSchedule))
                    {
                        pillSchedule.ResetSchedule();
                    }
                }

            }
            else
            {
                PillsSchedules.ForEach(p => p.ResetSchedule());
            }
            updateAllPillSchedule();
        }
        
        void checkingNextPileToTake(object _sender, ElapsedEventArgs _eventArgs)
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
        void updateAllPillSchedule()
        {
            foreach (var pillSchedule in PillsSchedules)
            {
                pillReminderIO.UpdateTaskeScheduleData(pillSchedule.ID, pillSchedule);
            }
        }


        public override void CheckingOffFinishedTask(Tuple<Pill,Time> pillTaken)
        {
            int pillScheduleIndex = PillsSchedules.FindIndex(p => p.Pill.Equals(pillTaken.Item1));

            if (pillScheduleIndex != -1)
            {
                int takenRecordForTheDayIndex = PillsSchedules[pillScheduleIndex].TakenRecordForTheDay.FindIndex(p => p.Item1.Equals(pillTaken.Item2));
                checkOffTakenTime(pillTaken, pillScheduleIndex, takenRecordForTheDayIndex);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pill","Pill given does not exist");
            }


        }

        private void checkOffTakenTime(Tuple<Pill, Time> pillTaken, int pillScheduleIndex, int takenRecordForTheDayIndex)
        {
            if (takenRecordForTheDayIndex != -1)
            {

                PillsSchedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex]
                    = new Tuple<Time, bool>(PillsSchedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex].Item1, true);
                CurrentTask.Remove(pillTaken);

                pillReminderIO.UpdateTaskeScheduleData(PillsSchedules[pillScheduleIndex].ID, PillsSchedules[pillScheduleIndex]);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Time", "Given pilltaken time does not exist in schedule");

            }
        }


    }
}
