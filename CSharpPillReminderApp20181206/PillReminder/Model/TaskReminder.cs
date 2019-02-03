using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reminder.Model;

namespace Reminder.Model
{
    public abstract class TaskReminderr <TTaskModel,TTaskScheduleModel,TTaskReminderEventArg> where TTaskReminderEventArg : EventArgs
    {    
        public abstract event EventHandler<TTaskReminderEventArg> OnNewTaskReminder;
        public abstract List<Tuple<TTaskModel, Time>> CurrentTask { get;  }
        public abstract void CheckingOffFinishedTask(Tuple<TTaskModel, Time> taskFinished);
    }
}
