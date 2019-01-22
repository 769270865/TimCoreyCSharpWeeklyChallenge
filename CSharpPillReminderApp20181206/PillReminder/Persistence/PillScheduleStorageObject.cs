using PillReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Persistence
{
    /// <summary>
    /// Persistence data storae object that flatten complex type for json storage
    /// </summary>
    public class PillScheduleStorageObject
    {
        public  Pill Pill;
        public  List<DateTime> TakenTimes;
        public  List<bool> HasTaken;


        public PillScheduleStorageObject() { }
            
            
        
        public PillScheduleStorageObject(PillSchedule pillSchedule)
        {
            TakenTimes = new List<DateTime>();
            HasTaken = new List<bool>();
            Pill = pillSchedule.Pill;

            foreach (var item in pillSchedule.TakenRecordForTheDay)
            {
                TakenTimes.Add(item.Item1.ToDateTime());
                HasTaken.Add(item.Item2);
            }

        }
        public PillSchedule PillScheduleStorageObjectToPillSchedule()
        {
            List<Tuple<Time, bool>> takenRecord = new List<Tuple<Time, bool>>();
            for (int i = 0; i < TakenTimes.Count; i++)
            {
                takenRecord.Add(new Tuple<Time, bool>(new Time(TakenTimes[i]), HasTaken[i])); 
            }
            PillSchedule pillSchedule = new PillSchedule(Pill, takenRecord);

            return pillSchedule;
        }
    }
}
