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
        /// <param name="timeToTake">Time pill should taken,give 0 hour if no pill time is later then the given time</param>
        /// <param name="Interval">Time intervaal to check,1/2 of interval before and after </param>
        /// <returns>is it time to take this pill</returns>
        public bool IsTimeToTake(Time time,out Time timeToTake,Time Interval)
        {
            for (int i = 0; i < TakenRecordForTheDay.Count; i++)
            {
                if (TakenRecordForTheDay[i].Item1.Ticks >= time.Ticks - Interval.Ticks/2 &&
                    TakenRecordForTheDay[i].Item1.Ticks <= time.Ticks + Interval.Ticks/2)
                {
                    timeToTake = TakenRecordForTheDay[i].Item1;
                    if (!TakenRecordForTheDay[i].Item2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   
                }
            }
          
            List<Tuple<Time, bool>> takenRecordAfterCurrentTime = TakenRecordForTheDay
                .FindAll(p => p.Item1.Ticks > time.Ticks)
                .OrderByDescending(p => p.Item1.Ticks)                
                .ToList();

            if (takenRecordAfterCurrentTime.Count > 0)
            {
                timeToTake = takenRecordAfterCurrentTime.Last().Item1;
            }
            else
            {
                timeToTake = new Time(0, 0, 0);
            }
           
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
