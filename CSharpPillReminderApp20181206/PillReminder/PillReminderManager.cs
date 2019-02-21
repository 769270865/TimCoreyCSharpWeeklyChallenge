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
    public class PillReminderManager : TaskReminderr<Pill, PillSchedule, PillReminderEventArg>
    {
        List<PillSchedule> _schedules;
        public override List<PillSchedule> Schedules
        {
            get
            {
                return _schedules;
            }
        }

        public override event EventHandler<PillReminderEventArg> OnNewTaskReminder;

        List<Tuple<Pill, Time>> _currentTask;
        public override List<Tuple<Pill, Time>> CurrentTask
        {
            get
            {
                return _currentTask;
            }
        }

        Time checkingInterval;

        ITaskReminderIO<Pill, PillSchedule> pillReminderIO;
        ITimeProvider timeProvider;
        ITimer pillReminderCheckingTimer;


        const long savingIntervalTicks = 18000000000;
        
        int currentIntervalWithoutUpdateCount, updateInterval;



        public PillReminderManager(Time CheckingInterval, ITaskReminderIO<Pill, PillSchedule> PillReminderIO, ITimeProvider TimeProvider, ITimer TimerProvider)
        {
            _schedules = new List<PillSchedule>();
            _currentTask = new List<Tuple<Pill, Time>>();
            checkingInterval = CheckingInterval;

            setDependency(PillReminderIO, TimeProvider, TimerProvider);
            intializeReminderTimerData();

            _schedules = pillReminderIO.GetAllTaskSchedule();
        }

        private void setDependency(ITaskReminderIO<Pill, PillSchedule> PillReminderIO, ITimeProvider TimeProvider, ITimer timer)
        {

            pillReminderIO = PillReminderIO;
            timeProvider = TimeProvider;
            pillReminderCheckingTimer = timer;
        }

        private void intializeReminderTimerData()
        {
            intializePillCheckingTimer();
            calculatingSavingIntervalData();
        }

        private void intializePillCheckingTimer()
        {
            pillReminderCheckingTimer.Interval = checkingInterval.Ticks;
            pillReminderCheckingTimer.AutoReset = true;
            pillReminderCheckingTimer.Enabled = true;
            pillReminderCheckingTimer.Elapsed += onIntervalPassed;
            pillReminderCheckingTimer.Start();
        }

        void calculatingSavingIntervalData()
        {
            updateInterval = Math.Min(((int)(checkingInterval.Ticks / savingIntervalTicks)), 1);
            currentIntervalWithoutUpdateCount = 0;
        }

        void onIntervalPassed(object sender, ElapsedEventArgs eventArgs)
        {
            checkingNextPileToTake(sender, eventArgs);

            resetAllPillAtMidNight(sender, eventArgs);

            savingPillOnInterval(sender, eventArgs);
        }



        void checkingNextPileToTake(object _sender, ElapsedEventArgs _eventArgs)
        {

            List<Tuple<Pill, Time>> newPillsToTake = new List<Tuple<Pill, Time>>();

            foreach (var pill in Schedules)
            {
                Time takingTime;
                if (pill.IsTimeToTake(timeProvider.CurrentTime, out takingTime, checkingInterval))
                {

                    newPillsToTake.Add(new Tuple<Pill, Time>(pill.Pill, takingTime));

                }
            }

            _currentTask.AddRange(newPillsToTake);


            if (newPillsToTake.Count > 0)
            {
                OnNewTaskReminder?.Invoke(this, new PillReminderEventArg() { PillToTakeWithTime = newPillsToTake });
            }

        }
        void resetAllPillAtMidNight(object _sender, ElapsedEventArgs _eventArgs)
        {
            if (timeProvider.CurrentTime.Ticks > (Time.MAX_TICKS - checkingInterval.Ticks / 2) && timeProvider.CurrentTime.Ticks < (checkingInterval.Ticks / 2))
            {
                resetPillExcludeCurrentInterval();

            }
            else
            {
                return;
            }
        }

        private void savingPillOnInterval(object sender, ElapsedEventArgs eventArgs)
        {
            if (currentIntervalWithoutUpdateCount >= updateInterval)
            {
                pillReminderIO.UpdateTaskScheduleDatas(Schedules);

                currentIntervalWithoutUpdateCount = 0;
            }
            else
            {
                currentIntervalWithoutUpdateCount++;
            }

        }
        private void resetPillExcludeCurrentInterval()
        {
            IEnumerable<PillSchedule> pillToTakeAtCurrentInterval = Schedules
                                                            .FindAll(p => p.IsTimeToTake(timeProvider.CurrentTime, checkingInterval));

            if (pillToTakeAtCurrentInterval.Count() > 0)
            {
                foreach (var pillSchedule in Schedules)
                {
                    if (!pillToTakeAtCurrentInterval.Contains(pillSchedule))
                    {
                        pillSchedule.ResetSchedule();
                    }
                }

            }
            else
            {
                Schedules.ForEach(p => p.ResetSchedule());
            }
            updateAllPillSchedule();
        }



        void updateAllPillSchedule()
        {
            foreach (var pillSchedule in Schedules)
            {
                pillReminderIO.UpdateTaskScheduleData(pillSchedule);
            }
        }


        public override void CheckingOffFinishedTask(Tuple<Pill, Time> pillTaken)
        {
            int pillScheduleIndex = Schedules.FindIndex(p => p.Pill.Equals(pillTaken.Item1));

            if (pillScheduleIndex != -1)
            {
                int takenRecordForTheDayIndex = Schedules[pillScheduleIndex].TakenRecordForTheDay.FindIndex(p => p.Item1.Equals(pillTaken.Item2));
                checkOffTakenTime(pillTaken, pillScheduleIndex, takenRecordForTheDayIndex);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pill", "Pill given does not exist");
            }


        }

        private void checkOffTakenTime(Tuple<Pill, Time> pillTaken, int pillScheduleIndex, int takenRecordForTheDayIndex)
        {
            if (takenRecordForTheDayIndex != -1)
            {

                Schedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex]
                    = new Tuple<Time, bool>(Schedules[pillScheduleIndex].TakenRecordForTheDay[takenRecordForTheDayIndex].Item1, true);
                _currentTask.Remove(pillTaken);

                pillReminderIO.UpdateTaskScheduleData(Schedules[pillScheduleIndex]);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Time", "Given pilltaken time does not exist in schedule");

            }
        }


    }
}
