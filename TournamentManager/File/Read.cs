using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace TournamentManager
{

public class Read
    {
        public static ITournament Tournament( string name )
        {
            var path = Path(name);
            var str = File.ReadAllText(path + "\\tournament.json");
        
            Dictionary<string, string> tourDesc = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
            TEnum.TournamentDyscypline enumType = (TEnum.TournamentDyscypline) int.Parse(tourDesc["Dyscypline"]);
            ITournament t = new Tournament(tourDesc["Name"], enumType);
            Dictionary<int, TPerson.Referee> refDic = Referee(path);
            foreach(var x in refDic)
            {
                t.AddReferee(x.Value);
            }

        
  
            Dictionary<int, TTeam.ITeam> teamDic = Team(path,enumType);
            foreach (var x in teamDic)
            {
                t.AddTeam(x.Value);
            }

            


            return t;
        }

        public static Dictionary<int, TPerson.Referee> Referee(string path)
        {
            var str = File.ReadAllText(path + "\\referees.json");
            List<Dictionary<string, string>> refDesc = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(str);
            List<TPerson.Referee> refe = new List<TPerson.Referee>();
            Dictionary<int, TPerson.Referee> refDic = new Dictionary<int, TPerson.Referee>();

            for (int i = 0; i< refDesc.Count; i++)
            {
                refDic.Add(int.Parse(refDesc[i]["Id"]), new TPerson.Referee(refDesc[i]["Firstname"], refDesc[i]["Lastname"], byte.Parse(refDesc[i]["Age"]), byte.Parse(refDesc[i]["Id"])));
            }

            return refDic;
        }


        public static Dictionary<int, TTeam.ITeam> Team(string path, TEnum.TournamentDyscypline type)
        {
            var str = File.ReadAllText(path + "\\teams.json");
            List<TeamTempl> teamDesc;


            teamDesc = JsonConvert.DeserializeObject<List<TeamTempl>>(str );
            
            List<TTeam.ITeam> team = new List<TTeam.ITeam>();
            Dictionary<int, TTeam.ITeam> teamDic = new Dictionary<int, TTeam.ITeam>();

            switch (type) {
                case TEnum.TournamentDyscypline.volleyball:
                    for (int i = 0; i < teamDesc.Count; i++)
                    {
                        teamDic.Add(teamDesc[i].Id, new TTeam.VolleyballTeam(teamDesc[i].Name,teamDesc[i].Id, Players(teamDesc[i].listPlayers)));
                    }
                    break;

                default:
                    throw new TException.TournamentDyscyplineNotIdentify();
            }
            return teamDic;
        }
        
        private static List<TPerson.Player> Players(List<TeamTempl.Player> playerDesc)
        {
            
            List<TPerson.Player> players = new List<TPerson.Player>();
            
                    for (int i = 0; i < playerDesc.Count; i++)
                    {
                        players.Add( new TPerson.Player(playerDesc[i].Firstname, playerDesc[i].Lastname, playerDesc[i].Age, playerDesc[i].Number));
                    }
            return players;
        }
        

        private static string Path(string name)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\" + name;
        }

        private class TeamTempl
        {
            public List<Player> listPlayers;
            public int Points;
            public int ScoreDiff;
            public int Id;
            public string Name;
            public int MatchesPlayed;
            public int MatchesWon;

            public class Player
            {
                public byte Number;
                public byte Age;
                public string Firstname;
                public string Lastname;
            }
        }
    }
}
