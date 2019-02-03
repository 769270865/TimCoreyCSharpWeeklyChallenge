using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Model
{
    
    public struct Time : IEquatable<Time>
    {
        public int Hour { get; private set; }
        public int Minute { get; private set; }
        public int Second { get; private set; }
        public long Ticks { get; private set; }
        public const long MAX_TICKS = 863990000000;


        public Time(DateTime dateTime)
        {
            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
            Ticks = dateTime.TimeOfDay.Ticks;
            
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
            Ticks = ((long)second * 10000000) + ((long)minute * 60 * 10000000) + ((long)hour * 60 * 60 * 10000000);

        }
        public DateTime ToDateTime()
        {
            DateTime currentTime = DateTime.Now;
            DateTime time = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, Hour, Minute, 0);
            return time;
        }

        public override bool Equals(object obj)
        {
            return obj is Time && Equals((Time)obj);
        }

        public bool Equals(Time other)
        {
            return Hour == other.Hour &&
                   Minute == other.Minute &&
                   Second == other.Second;
                  
        }

        public override int GetHashCode()
        {
            var hashCode = 109327786;
            hashCode = hashCode * -1521134295 + Hour.GetHashCode();
            hashCode = hashCode * -1521134295 + Minute.GetHashCode();
            hashCode = hashCode * -1521134295 + Second.GetHashCode();
            
            return hashCode;
        }
    }
}
