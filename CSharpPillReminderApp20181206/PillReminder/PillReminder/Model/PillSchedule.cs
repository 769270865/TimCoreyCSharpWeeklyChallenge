using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Model
{
    [Serializable]
    public class PillSchedule
    {
        public Pill Pill { get; set; }
        public List<Tuple<Time,bool>> TakenRecordForTheDay { get; set; }
        

        // Time difference allowed for taken pills
        const int TIME_OF_ALLOWED_DIFFERENCE_MiNUTES = 5;
        const long TIME_OF_ALLOWED_DIFFERENCE_TICKS = (long)TIME_OF_ALLOWED_DIFFERENCE_MiNUTES * 60 * 10000000;

        public PillSchedule(Pill pill, List<Tuple<Time, bool>> takenRecordForTheDay)
        {
            Pill = pill;
            TakenRecordForTheDay = takenRecordForTheDay;
        }

        public bool IsTimeToTake(Time time,out Time timeToTake)
        {
            for (int i = 0; i < TakenRecordForTheDay.Count; i++)
            {
                if (TakenRecordForTheDay[i].Item1.Ticks > TakenRecordForTheDay[i].Item1.Ticks - TIME_OF_ALLOWED_DIFFERENCE_TICKS &&
                    TakenRecordForTheDay[i].Item1.Ticks < TakenRecordForTheDay[i].Item1.Ticks + TIME_OF_ALLOWED_DIFFERENCE_TICKS)
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
    }
}
