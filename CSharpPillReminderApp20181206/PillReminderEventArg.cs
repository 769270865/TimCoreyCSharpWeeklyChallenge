using PillReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder
{
    public class PillReminderEventArg : EventArgs
    {
        public List<Tuple<Pill,Time>> PillToTakeWithTime { get; set; }
    }
}
