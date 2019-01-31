using Reminder.Model;
using Reminder.PillReminnder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.PillReminnder.Persistence
{
    /// <summary>
    /// Persistence data storae object that flatten complex type for json storage
    /// </summary>
    public class PillScheduleStorageObject
    {
        public Guid ID;
        public  Pill Pill;
        public  List<DateTime> TakenTimes;
        public  List<bool> HasTaken;


        public PillScheduleStorageObject() { }
            
            
        
        public PillScheduleStorageObject(PillSchedule pillSchedule)
        {
            ID = pillSchedule.ID;
            Pill = pillSchedule.Pill;
            TakenTimes = new List<DateTime>();
            HasTaken = new List<bool>();
            

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
            PillSchedule pillSchedule = new PillSchedule(ID,Pill, takenRecord);

            return pillSchedule;
        }
    }
}
