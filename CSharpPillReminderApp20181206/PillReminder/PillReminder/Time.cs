using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder
{
    [Serializable]
    public struct Time
    {
        public int Hour { get; private set; }
        public int Minute { get; private set; }
        public int Second { get; private set; }

        public Time(DateTime dateTime)
        {
            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Minute;
        }
        public Time(int hour, int minute,int second)
        {
            if (hour > 23 || hour < 0)
            {
                throw new ArgumentException("Hour should be a number between 0 and 23");
            }
            if (minute > 59 || minute < 0 )
            {
                throw new ArgumentException("Minute should be a number between 0 and 59");
            }
            if (second > 59 || second < 0)
            {
                throw new ArgumentException("Second should be a number between 0 and 59");
            }
            Hour = hour; Minute = minute; Second = second;
        }
        

    }
}
