using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using TournamentManager.TRound;
using System.Runtime.Serialization;
using TournamentManager.TException;
using System.Text.RegularExpressions;

namespace TournamentManager
{

    public class Read
    {
        public static ITournament Tournament(string name)
        {
            var path = Path(name);
            var str = File.ReadAllText(path + "\\tournament.json");

            Dictionary<string, string> tourDesc = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
            TEnum.TournamentDyscypline enumType = (TEnum.TournamentDyscypline)int.Parse(tourDesc["Dyscypline"]);
            ITournament t = new Tournament(tourDesc["Name"], enumType);


            Dictionary<int, TPerson.Referee> refDic = Referee(path);
            foreach (var x in refDic)
            {
                t.AddReferee(x.Value);
            }


            Dictionary<int, TTeam.ITeam> teamDic = Team(path, enumType);
            foreach (var x in teamDic)
            {
                t.AddTeam(x.Value);
            }

            t.League = League(path, refDic, teamDic, enumType);
            t.League.Teams = t.Teams;
            t.League.Referees = t.Referees;

            t.PlayOff = PlayOff(path, refDic, teamDic, enumType);

            return t;
        }

        public static TRound.PlayOff PlayOff(string path, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams, TEnum.TournamentDyscypline tenum)
        {
            TRound.PlayOff p = new TRound.PlayOff();

            var json = JsonConvert.DeserializeObject<Dictionary<string, List<RoundTempl>>>(File.ReadAllText(path + "\\playoff.json"))["Rounds"];
            List<TRound.Round> rP = new List<TRound.Round>();
            RoundTempl elem;
            TRound.Round round;
            TMatch.Match match;
            int j;
            for (int i = 0; i < json.Count; i++)
            {
                elem = json[i];
               
                rP.Add(Round(elem, tenum, referees,teams));
            }

            foreach(var x in referees)
            {
                p.addReferee(x.Value);
            }

            p.Rounds = rP;
            return p;
        }


        public static TRound.League League(string path, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams, TEnum.TournamentDyscypline tenum)
        {
            TRound.League l = new TRound.League();

            var json = (JsonConvert.DeserializeObject<Dictionary<string, List<RoundTempl>>>(File.ReadAllText(path + "\\league.json")))["Rounds"];

            List<TRound.Round> rL = new List<TRound.Round>();
            RoundTempl elem;
           
            
            for (int i = 0; i < json.Count; i++)
            {
                elem = json[i];
                
                rL.Add(Round(elem,tenum,referees, teams));
            }
            l.Rounds = rL;
            return l;
        }

        private static TRound.Round Round(RoundTempl elem, TEnum.TournamentDyscypline tenum, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams)
        {
            int j;
            TMatch.Match match;
            TRound.Round round = new TRound.Round(elem.RoundName, elem.Date);
            switch (tenum)
            {
                case TEnum.TournamentDyscypline.dodgeball:
                    for (j = 0; j < elem.ListMatches.Count; j++)
                    {
                        match = CreateMatch(elem.ListMatches[j], tenum, referees, teams);
                        ((TMatch.DodgeballMatch)match).SetResult(elem.ListMatches[j].winnerPlayersLeft);
                        round.AddMatch(match);
                    }
                    break;
                case TEnum.TournamentDyscypline.tugofwar:
                    for (j = 0; j < elem.ListMatches.Count; j++)
                    {

                        match = CreateMatch(elem.ListMatches[j], tenum, referees, teams);
                        ((TMatch.TugOfWarMatch)match).SetResult(elem.ListMatches[j].matchLength);
                        round.AddMatch(match);
                    }
                break;
                case TEnum.TournamentDyscypline.volleyball:
                    for (j = 0; j < elem.ListMatches.Count; j++)
                {
                        match = CreateVolleyballMatch(elem.ListMatches[j], tenum, referees, teams);
                    round.AddMatch(match);
                }

                break;
                default: throw new TournamentDyscyplineNotIdentify();
            }
            return round;
        }

        private static TMatch.Match CreateVolleyballMatch(MatchTempl elem, TEnum.TournamentDyscypline tenum, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams)
        {
            TMatch.Match match = new TMatch.VolleyballMatch(teams[elem.TeamA], teams[elem.TeamB], new List<TPerson.Referee>{
                                referees[elem.RefA],
                                referees[elem.assistantReferees[0]],
                                referees[elem.assistantReferees[1]]
                            });

            ((TMatch.VolleyballMatch)match).SetResult(elem.ScoreTeamA, elem.ScoreTeamB);

            if (elem.Winner != null)
            {
                match.Winner = teams[int.Parse(elem.Winner)];

            }

            return match;
        }

        private static TMatch.Match CreateMatch(MatchTempl elem, TEnum.TournamentDyscypline tenum, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams)
        {
            TMatch.Match match;
            switch (tenum) {
                case TEnum.TournamentDyscypline.dodgeball:
                    
                     match = new TMatch.DodgeballMatch(teams[elem.TeamA], teams[elem.TeamB], new List<TPerson.Referee>{
                        referees[elem.RefA]
                    });
                break;
                default: return null;
            }

            if (elem.Winner != null)
            {
                match.Winner = teams[int.Parse(elem.Winner)];

            }  

            return match;
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



        // template class using tp deserialize from file  

        private class TeamTempl
        {
            public List<Player> listPlayers=null;
            public int Points = 0;
            public int ScoreDiff = 0;
            public int Id = 0;
            public string Name = null;
            public int MatchesPlayed = 0;
            public int MatchesWon = 0;

            public class Player
            {
                public byte Number = 0;
                public byte Age = 0;
                public string Firstname = null;
                public string Lastname = null;
            }
        }

        private class RoundTempl
        {
            public int[] Date = null;
            public string RoundName = null;
            public List<MatchTempl> ListMatches = null;
            
        }

        private class MatchTempl
        {
            public int[] assistantReferees = null;
            public int[] ScoreTeamA = null;
            public int[] ScoreTeamB = null;
            public float matchLength = 0;
            public int winnerPlayersLeft = 0;
            public int TeamA = 0;
            public int TeamB = 0;
            public string Winner = null;
            public int RefA = 0;


        }
    }
}
