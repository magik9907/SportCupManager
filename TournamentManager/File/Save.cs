using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{

    /// <summary>
    /// static class to save to json file
    /// </summary>
    public class Save
    {
        /// <summary>
        /// save all content from tournament object
        /// </summary>
        /// <param name="t">tournament object</param>
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
                {}
                try
                {
                    Teams(t.Teams, name);
                }
                catch (Exception) 
                {}

                try
                {
                    League(t.League, name);
                }catch(Exception)
                { }

                try
                {
                    PlayOff(t.PlayOff, name);
                }catch(Exception)
                {}
                
            }
            catch (Exception e)
            {}
        }

        /// <summary>
        /// save referees to json file (referees.json)
        /// </summary>
        /// <param name="r"> list of referees object</param>
        /// <param name="name">tournament name as string</param>
        public static void Referees(List<TPerson.Referee> r , string name = null)
        {
            IsNameAndObject(name, r, "TPerson.Referee");

            var path = Path(name);

            SerializeObject(path, "referees.json", r);
        }

        public static void TournamentObject(ITournament t, string name = null)
        {
            IsNameAndObject(name, t, "Tournament");

            var path = Path(name);
            SerializeObject(path, "tournament.json", t);
        }

        /// <summary>
        /// save teams to json file (teams.json)
        /// </summary>
        /// <param name="t"> list of teams object</param>
        /// <param name="name">tournament name as string</param>
        public static void Teams(List<TTeam.ITeam> t , string name = null)
        {
            IsNameAndObject(name, t, "TTeam.ITeam");

            var path = Path(name);

            SerializeObject(path, "teams.json", t);
        }

        /// <summary>
        /// save leagues to json file (league.json)
        /// </summary>
        /// <param name="l"> league object</param>
        /// <param name="name">tournament name as string</param>
        public static void League(TRound.League l , string name = null)
        {
            IsNameAndObject(name, l, "TRound.League");

            var path = Path(name);

            SerializeObject(path, "league.json", l);
        }
        /// <summary>
        /// save playoff to json file (playoff.json)
        /// </summary>
        /// <param name="p"> playoff object</param>
        /// <param name="name">tournament name as string</param>
        public static void PlayOff(TRound.PlayOff p , string name = null)
        {
            IsNameAndObject(name, p, "TRound.PlayOff");

            var path = Path(name); 

            SerializeObject(path, "playoff.json", p);
        }

        /// <summary>
        /// set path to tournament directory data in my documents
        /// </summary>
        /// <param name="name">tournament name</param>
        /// <returns>path as string</returns>
        private static string Path(string name)
        {
            return (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\" + name).Replace(" ", "");
        }

        /// <summary>
        /// method to serialize object in indented mode
        /// </summary>
        /// <param name="path">path as string to directory</param>
        /// <param name="fileName">file name to save</param>
        /// <param name="obj">object to save</param>
        private static void SerializeObject(string path, string fileName, Object obj)
        {
            SerializeObject(path, fileName, obj, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        /// <summary>
        /// method to serialize object
        /// </summary>
        /// <param name="path">path as string to directory</param>
        /// <param name="fileName">file name to save</param>
        /// <param name="obj">object to save</param>
        /// <param name="js">additional settings to serialize</param>
        private static void SerializeObject(string path, string fileName, Object obj, JsonSerializerSettings js)
        {
            FileExist(path, fileName);
            File.WriteAllText(path + "\\" + fileName, JsonConvert.SerializeObject(obj, js));
        }

        /// <summary>
        /// check if file/directory exist. if not create directory/file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
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

        /// <summary>
        /// check if name and object is setted
        /// </summary>
        /// <param name="name">tournament name</param>
        /// <param name="obj">object to save</param>
        /// <param name="expObj">escpetion object name if there is an exception</param>
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
