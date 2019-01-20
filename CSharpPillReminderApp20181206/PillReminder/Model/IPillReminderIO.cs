using PillReminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillReminder.Model
{
    public interface IPillReminderIO
    {
        void SavePillData(Pill pill);
        List<Pill> GetAllPill();
        void SavePillSchedule(PillSchedule pillSchedule);
        List<PillSchedule> GetAllPillSchedule();

    }
}
