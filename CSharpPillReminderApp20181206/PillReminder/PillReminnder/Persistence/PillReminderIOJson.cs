using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reminder.Model;
using Reminder.PillReminnder.Model;

namespace Reminder.PillReminnder.Persistence
{
    public class PillReminderIOJson : ITaskReminderIO<Pill,PillSchedule>
    {
        string persistenceFolderPath,pillDataFolderPath,pillScheduleDataFolderPath;

        public PillReminderIOJson(string PersistencePath)
        {
            persistenceFolderPath = PersistencePath;

            pillDataFolderPath = Path.Combine(persistenceFolderPath, "PillData");
            pillScheduleDataFolderPath = Path.Combine(persistenceFolderPath, "PillScheduleData");

            Directory.CreateDirectory(pillDataFolderPath);
            Directory.CreateDirectory(pillScheduleDataFolderPath);
        }
        public List<Pill>GetAllTask()
        {
            List<Pill> allPills = new List<Pill>();
            JsonSerializer serializer = new JsonSerializer();

            string[] PillsJsonFilesPath = Directory.GetFiles(pillDataFolderPath, "*.dat");

            foreach (var filePath in PillsJsonFilesPath)
            {
                using (StreamReader sr = new StreamReader(filePath))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    allPills.Add(serializer.Deserialize<Pill>(reader));
                }
                    
            }
            return allPills;
        }
        public List<PillSchedule> GetAllTaskSchedule()
        {
            List<PillSchedule> pillSchedules = new List<PillSchedule>();
            List<PillScheduleStorageObject> pillScheduleStorageObjects = new List<PillScheduleStorageObject>();

            JsonSerializer serializer = new JsonSerializer();
            string[] pillScheduleDataFilesPath = Directory.GetFiles(pillScheduleDataFolderPath, "*.dat");
            foreach (var filePaath in pillScheduleDataFilesPath)
            {
                using (StreamReader sr = new StreamReader(filePaath))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    PillScheduleStorageObject pillScheduleStorageObject = serializer.Deserialize<PillScheduleStorageObject>(reader);
                    pillScheduleStorageObjects.Add(pillScheduleStorageObject);
                }
            }
            pillScheduleStorageObjects.ForEach(p => pillSchedules.Add(p.PillScheduleStorageObjectToPillSchedule()));

            return pillSchedules;
        }

        public void SaveTaskData(Pill pill)
        {
            JsonSerializer seralizer = new JsonSerializer();

            saveTaskData(pill, seralizer);
                
        }
        public void SaveTaskDatas(List<Pill> taskModels)
        {
            JsonSerializer serializer = new JsonSerializer();

            foreach (var task in taskModels)
            {
                saveTaskData(task, serializer);
            }
        }
        void saveTaskData(Pill pill,JsonSerializer serializer)
        {
            using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{pill.ID.ToString()}.dat", false))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, pill);
            }
        }
        

        public void SaveTaskScheduleData(PillSchedule pillSchedule)
        {
            JsonSerializer serializer = new JsonSerializer();
            saveTaskScheduleData(pillSchedule, serializer);

        }
        public void SaveTaskScheduleDatas(List<PillSchedule> taskSchedulModeles)
        {
            JsonSerializer serializer = new JsonSerializer();

            foreach (var taskSchedule in taskSchedulModeles)
            {
                saveTaskScheduleData(taskSchedule, serializer);
            }
        }
        private void saveTaskScheduleData(PillSchedule pillSchedule, JsonSerializer serializer)
        {
            PillScheduleStorageObject pillScheduleStorageObject = new PillScheduleStorageObject(pillSchedule);

            using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{pillSchedule.ID.ToString()}_Schedule.dat"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, pillScheduleStorageObject);
            }
        }

        

        public void UpdateTaskData(Pill taskModel)
        {

            if (File.Exists($@"{pillDataFolderPath}\{taskModel.ID.ToString()}.dat"))
            {
                JsonSerializer serializer = new JsonSerializer();
                saveTaskData(taskModel, serializer);
            }
            else
            {
                throw new ArgumentException("Specificed file does not exist");
            }
        }
        public void UpdateTaskDatas(List<Pill> taskModels)
        {
            for (int i = 0; i < taskModels.Count; i++)
            {
                try
                {
                    UpdateTaskData(taskModels[i]);
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException($"Task model index : {i} does not exist");
                    
                }              
            }
        }
        public void UpdateTaskScheduleData(PillSchedule taskScheduleModel)
        {
           
            if (File.Exists($@"{pillScheduleDataFolderPath}\{taskScheduleModel.ID.ToString()}_Schedule.dat"))
            {
                JsonSerializer serializer = new JsonSerializer();
                saveTaskScheduleData(taskScheduleModel, serializer);
            }
            else
            {
                throw new ArgumentException("Specificed file does not exist");
            }
        }
        public void UpdateTaskScheduleDatas(List<PillSchedule> taskScheduls)
        {
            for (int i = 0; i < taskScheduls.Count; i++)
            {
                try
                {
                    UpdateTaskScheduleData(taskScheduls[i]);
                }
                catch (ArgumentException)
                {

                    throw new ArgumentException($"Pill schedule index : {i} does not exist");
                }
            }
        }

        public void DeleteTaskData(Pill taskModelID)
        {
            if (File.Exists($@"{pillDataFolderPath}\{taskModelID.ID.ToString()}.dat"))
            {
                File.Delete($@"{pillDataFolderPath}\{taskModelID.ID.ToString()}.dat");
            }
        }
        public void DeleteTaskDatas(List<Pill> taskModels)
        {
            foreach (var pill in taskModels)
            {
                DeleteTaskData(pill);
            }
        }
 
        public void DeleteTaskScheduleData(PillSchedule taskScheduleModel)
        {
            if (File.Exists($@"{pillScheduleDataFolderPath}\{taskScheduleModel.ID.ToString()}_Schedule.dat"))
            {
                File.Delete($@"{pillScheduleDataFolderPath}\{taskScheduleModel.ID.ToString()}_Schedule.dat");
            }
            
        }
        public void DeleteTaskScheduleDatas(List<PillSchedule> taskSchedulModeles)
        {
            foreach (var pillSchedule in taskSchedulModeles)
            {
                DeleteTaskScheduleData(pillSchedule);
            }
        }

      
    }
}
