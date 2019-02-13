using Reminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Model
{
    public interface ITaskReminderIO<TTaskModel,TTaskSchedulModele>
    {
        List<TTaskModel> GetAllTask();
        List<TTaskSchedulModele> GetAllTaskSchedule();
        

        void SaveTaskData(TTaskModel taskModel);
        void SaveTaskDatas(List<TTaskModel> taskModels);

        void SaveTaskScheduleData(TTaskSchedulModele pillSchedule);
        void SaveTaskScheduleDatas(List<TTaskSchedulModele> taskSchedulModeles);

        void UpdateTaskData(TTaskModel taskModel);
        void UpdateTaskDatas(List<TTaskModel> taskModels);
        void UpdateTaskScheduleData(TTaskSchedulModele taskModel);
        void UpdateTaskScheduleDatas(List<TTaskSchedulModele> taskScheduls);

        void DeleteTaskData(TTaskModel taskModel);
        void DeleteTaskDatas(List<TTaskModel> taskModels);
        void DeleteTaskScheduleData(TTaskSchedulModele taskScheduleID);
        void DeleteTaskScheduleDatas(List<TTaskSchedulModele> taskSchedulModeles);

    }
}
