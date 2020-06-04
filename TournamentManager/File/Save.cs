using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{
    public class Save
    {
        public static void Tournament(ITournament t = null)
        {
            if(t == null)
            {
                throw new TException.TournamentMustBeDefine();
            }

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\" + t.Name;

            SerializeObject(path , "tournament.json", t);
            SerializeObject(path , "referees.json", t.Referees);
            SerializeObject(path , "teams.json", t.Teams);

        }

        private static void SerializeObject(string path, string fileName, Object obj)
        {
            SerializeObject(path,fileName,obj, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        private static void SerializeObject(string path, string fileName, Object obj, JsonSerializerSettings js)
        {
            FileExist(path, fileName);
            File.WriteAllText(path + "\\" + fileName, JsonConvert.SerializeObject(obj, js));
        }

        private static void FileExist(string path, string fileName)
        {
            if (!(File.Exists(path + "\\" + fileName)))
            {
                if (!(Directory.Exists(path)))
                    Directory.CreateDirectory(path);
                FileStream f = File.Create(path + "\\" + fileName);
                f.Close();
            }
        }

    }
}
