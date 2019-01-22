using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PillReminder.Model;

namespace PillReminder.Utlitie
{
    public  class DateTimeProvider : ITimeProvider
    {
        public DateTime CurrrentDateTime => DateTime.Now;
        public Time CurrentTime => new Time(DateTime.Now);

        
    }
}
