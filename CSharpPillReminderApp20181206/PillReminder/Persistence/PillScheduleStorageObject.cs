using PillReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Persistence
{
    public class PillScheduleStorageObject
    {
        public readonly Pill Pill;
        public readonly List<DateTime> TakenTime;
        public readonly List<bool> HasTaken;

        public PillScheduleStorageObject(PillSchedule pillSchedule)
        {
            TakenTime = new List<DateTime>();
            HasTaken = new List<bool>();
            Pill = pillSchedule.Pill;

            foreach (var item in pillSchedule.TakenRecordForTheDay)
            {
                TakenTime.Add(item.Item1.ToDateTime());
                HasTaken.Add(item.Item2);
            }

        }
        public PillSchedule PillScheduleStorageObjectToPillSchedule()
        {
            throw new NotImplementedException();
        }
    }
}
