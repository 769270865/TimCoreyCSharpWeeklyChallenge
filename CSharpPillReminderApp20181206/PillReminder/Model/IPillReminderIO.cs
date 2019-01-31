using Reminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Model
{
    public interface ITaskReminderIO<TTaskModel,TTaskSchedule,TTaskModelID,TTaskScheduleID>
    {
        void SaveTaskData(TTaskModel pill);
        List<TTaskModel> ReadAllTask();
        void UpdateTaskData(TTaskModelID taskModelID);
        void DeleteTaskData(TTaskModelID taskModelID);

        void SaveTaskSchedule(TTaskSchedule pillSchedule);
        List<TTaskSchedule> GetAllTaskSchedule();
        void UpdateTaskeScheduleData(TTaskScheduleID taskScheduleID);
        void DeleteTaskData(TTaskScheduleID taskScheduleID);

    }
}
