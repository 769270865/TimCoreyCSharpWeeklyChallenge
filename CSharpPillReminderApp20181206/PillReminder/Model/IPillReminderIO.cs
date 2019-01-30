using Reminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Model
{
    public interface IPillReminderIO<TTaskModel,TTaskSchedule>
    {
        void SaveTaskData(TTaskModel pill);
        List<TTaskModel> GetAllTask();
        void SaveTaskSchedule(TTaskSchedule pillSchedule);
        List<TTaskSchedule> GetAllTaskSchedule();

    }
}
