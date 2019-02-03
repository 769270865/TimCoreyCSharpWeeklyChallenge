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
    public class PillReminderIOJson : ITaskReminderIO<Pill,PillSchedule,Guid,Guid>
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
        public void SaveTaskData(Pill pill)
        {
            JsonSerializer seralizer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{pill.ID.ToString()}.txt",false))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                seralizer.Serialize(writer, pill);
            }
                
        }
        public List<Pill>ReadAllTask()
        {
            List<Pill> allPills = new List<Pill>();
            JsonSerializer serializer = new JsonSerializer();

            string[] PillsJsonFilesPath = Directory.GetFiles(pillDataFolderPath, "*.txt");

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

        public void SaveTaskSchedule(PillSchedule pillSchedule)
        {
            PillScheduleStorageObject pillScheduleStorageObject = new PillScheduleStorageObject(pillSchedule);

            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{pillSchedule.ID.ToString()}_Schedule.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, pillScheduleStorageObject);
            }

        }
        public List<PillSchedule> GetAllTaskSchedule()
        {
            List<PillSchedule> pillSchedules = new List<PillSchedule>();
            List<PillScheduleStorageObject> pillScheduleStorageObjects = new List<PillScheduleStorageObject>();

            JsonSerializer serializer = new JsonSerializer();
            string[] pillScheduleDataFilesPath = Directory.GetFiles(pillScheduleDataFolderPath, "*.txt");
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

        public void UpdateTaskData(Guid taskModelID,Pill taskModel)
        {
            if (taskModelID != taskModel.ID )
            {
                throw new ArgumentException("ID of the pill model can not be changed");
            }

            if (File.Exists($@"{pillDataFolderPath}\{taskModelID}.dat"))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{taskModelID}.dat", false))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, taskModel);
                }        
            }
            else
            {
                throw new ArgumentException("Specificed file does not exist");
            }
        }

        public void DeleteTaskData(Guid taskModelID)
        {
            if (File.Exists($@"{pillDataFolderPath}\{taskModelID.ToString()}.dat"))
            {
                File.Delete($@"{pillDataFolderPath}\{taskModelID.ToString()}.dat");
            }
        }

        public void UpdateTaskeScheduleData(Guid taskScheduleID, PillSchedule taskModel)
        {
            if (taskScheduleID != taskModel.ID)
            {
                throw new ArgumentException("ID of the task schedule model can not be changed");
            }
            if (File.Exists($@"{pillScheduleDataFolderPath}\{taskScheduleID.ToString()}_Schedule.dat"))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{taskScheduleID.ToString()}_Schedule.dat"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, new PillScheduleStorageObject(taskModel));
                }
            }
            else
            {
                throw new ArgumentException("Specificed file does not exist");
            }
        }

        public void DeleteTaskScheduleData(Guid taskScheduleID)
        {
            if (File.Exists($@"{pillScheduleDataFolderPath}\{taskScheduleID.ToString()}_Schedule.dat"))
            {
                File.Delete($@"{pillScheduleDataFolderPath}\{taskScheduleID.ToString()}_Schedule.dat");
            }
            else
            {
                throw new ArgumentException("Specificed file does not exist");
            }
        }
    }
}
