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
            if (t == null)
            {
                throw new TException.TournamentMustBeDefine();
            }

            var path = Path(t.Name);

            var name = t.Name;
            try
            {
                SerializeObject(path, "tournament.json", t);

                try
                {
                    Referees(t.Referees, name);
                }
                catch (Exception)
                {

                }
                try
                {
                    Teams(t.Teams, name);
                }
                catch (Exception) 
                {
                
                }

                try
                {
                    League(t.League, name);
                }catch(Exception)
                {

                }
                try
                {


                    PlayOff(t.PlayOff, name);
                }catch(Exception)
                {

                }
                
            }
            catch (Exception e)
            {

            }
        }

        public static void Referees(List<TPerson.Referee> r , string name = null)
        {
            IsNameAndObject(name, r, "TPerson.Referee");

            var path = Path(name);

            SerializeObject(path, "referees.json", r);
        }

        public static void Teams(List<TTeam.ITeam> t , string name = null)
        {
            IsNameAndObject(name, t, "TTeam.ITeam");

            var path = Path(name);

            SerializeObject(path, "teams.json", t);
        }

        public static void League(TRound.League l , string name = null)
        {
            IsNameAndObject(name, l, "TRound.League");

            var path = Path(name);

            SerializeObject(path, "league.json", l);
        }

        public static void PlayOff(TRound.PlayOff p , string name = null)
        {
            IsNameAndObject(name, p, "TRound.PlayOff");

            var path = Path(name); 

            SerializeObject(path, "playoff.json", p);
        }

        private static string Path(string name)
        {
           return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\" + name;
        }


        private static void SerializeObject(string path, string fileName, Object obj)
        {
            SerializeObject(path, fileName, obj, new JsonSerializerSettings
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

        private static void IsNameAndObject(string name, object obj, string expObj)
        {
            if (name == null)
            {
                TournamentNameMustBeDefine();
            }
            if (obj == null)
            {
                ObjectIsNotDefine(expObj);
            }
        }

        private static void ObjectIsNotDefine(string objName)
        {
            throw new TException.ObjectNotDefined(objName);
        }
        private static void TournamentNameMustBeDefine()
        {
            throw new TException.TournamentNameMustBeDefine();
        }

    }
}
