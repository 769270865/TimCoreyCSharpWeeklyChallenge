using Reminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Model
{
    public interface ITaskReminderIO<TTaskModel,TTaskSchedulModele,TTaskModelID,TTaskScheduleModelID>
    {
        void SaveTaskData(TTaskModel pill);
        List<TTaskModel> ReadAllTask();
        void UpdateTaskData(TTaskModelID taskModelID,TTaskModel taskModel);
        void DeleteTaskData(TTaskModelID taskModelID);

        void SaveTaskSchedule(TTaskSchedulModele pillSchedule);
        List<TTaskSchedulModele> GetAllTaskSchedule();
        void UpdateTaskeScheduleData(TTaskScheduleModelID taskScheduleID,TTaskSchedulModele taskModel);
        void DeleteTaskScheduleData(TTaskScheduleModelID taskScheduleID);


    }
}
