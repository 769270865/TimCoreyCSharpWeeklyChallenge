using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder
{
    public class Pill
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public List<DateTime> TakingSchedule { get; set; }

    }
}
