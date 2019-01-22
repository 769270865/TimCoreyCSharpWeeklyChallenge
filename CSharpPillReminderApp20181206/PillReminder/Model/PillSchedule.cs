using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Model
{
    
    public class PillSchedule : IEquatable<PillSchedule>
    {
        public Pill Pill { get; set; }
        public List<Tuple<Time,bool>> TakenRecordForTheDay { get; set; }
        

        
        public PillSchedule(Pill pill, List<Tuple<Time, bool>> takenRecordForTheDay)
        {
            Pill = pill;
            TakenRecordForTheDay = takenRecordForTheDay;
        }
        
        /// <summary>
        /// Check if it is the time to take this pill
        /// </summary>
        /// <param name="time">Current time</param>
        /// <param name="timeToTake">Time pill should taken</param>
        /// <param name="Interval">Time intervaal to check</param>
        /// <returns>Is this pill should be taken</returns>
        public bool IsTimeToTake(Time time,out Time timeToTake,Time Interval)
        {
            for (int i = 0; i < TakenRecordForTheDay.Count; i++)
            {
                if (TakenRecordForTheDay[i].Item1.Ticks > time.Ticks - Interval.Ticks &&
                    TakenRecordForTheDay[i].Item1.Ticks < time.Ticks + Interval.Ticks)
                {
                    timeToTake = TakenRecordForTheDay[i].Item1;
                    return true;
                }
            }
            timeToTake = TakenRecordForTheDay.FindAll(p => p.Item1.Ticks > DateTime.Now.TimeOfDay.Ticks)
                         .OrderByDescending(x => x.Item1.Ticks).ToList().FirstOrDefault().Item1;

            return false;
        }


        public void ResetSchedule()
        {
            for (int i = 0; i < TakenRecordForTheDay.Count; i++)
            {
                TakenRecordForTheDay[i] = new Tuple<Time, bool>(TakenRecordForTheDay[i].Item1, false);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PillSchedule);
        }

        public bool Equals(PillSchedule other)
        {
            if (other == null)
                return false;
             
            if (!Pill.Equals(other.Pill))               
                return false;
            

            if (TakenRecordForTheDay.Count != other.TakenRecordForTheDay.Count)
            {
                return false; 
            }
            else
            {
                for (int i = 0; i < other.TakenRecordForTheDay.Count; i++)
                {
                    if (!other.TakenRecordForTheDay[i].Item1.Equals(TakenRecordForTheDay[i].Item1))
                    {
                        return false;
                    }
                }
            }

            return true; 

        }

        public override int GetHashCode()
        {
            var hashCode = 2116795969;
            hashCode = hashCode * -1521134297 + Pill.GetHashCode();
            hashCode = hashCode * -1521134295 + TakenRecordForTheDay.GetHashCode();
            return hashCode;
        }
    }
}
