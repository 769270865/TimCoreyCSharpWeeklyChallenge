using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reminder.Model;

namespace Reminder.Utlitie
{
    public  class DateTimeProvider : ITimeProvider
    {
        public DateTime CurrrentDateTime => DateTime.Now;
        public Time CurrentTime => new Time(DateTime.Now);

        
    }
}
