using Reminder.Model;
using Reminder.PillReminnder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder
{
    public class PillReminderEventArg : EventArgs
    {
        public List<Tuple<Pill,Time>> PillToTakeWithTime { get; set; }
    }
}
