using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PillReminder.Model;

namespace PillReminder.Persistence
{
    public class PillReminderIOJson : IPillReminderIO
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
        public void SavePillData(Pill pill)
        {
            JsonSerializer seralizer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter($@"{pillDataFolderPath}\{pill.Name}.txt",false))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                seralizer.Serialize(writer, pill);
            }
                
        }
        public List<Pill>GetAllPill()
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

        public void SavePillSchedule(PillSchedule pillSchedule)
        {
            PillScheduleStorageObject pillScheduleStorageObject = new PillScheduleStorageObject(pillSchedule);

            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter($@"{pillScheduleDataFolderPath}\{pillSchedule.Pill.Name}_Schedule.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, pillScheduleStorageObject);
            }

        }
        public List<PillSchedule> GetAllPillSchedule()
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
    }
}
