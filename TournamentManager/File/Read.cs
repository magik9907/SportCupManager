using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using TournamentManager.TRound;
using System.Runtime.Serialization;
using TournamentManager.TException;
using System.Text.RegularExpressions;
using System.Linq;

namespace TournamentManager
{
    /// <summary>
    /// static class, read data from file in json format
    /// </summary>
    public class Read
    {
        /// <summary>
        /// Read all tournament object from files
        /// </summary>
        /// <param name="name">name of tournament, required to find files</param>
        /// <returns></returns>
        public static ITournament Tournament(string name)
        {
            ///get path to directory (my document)
            var path = Path(name);
            var str = File.ReadAllText(path + "\\tournament.json");
            //tournamne properties as dictionary
            Dictionary<string, string> tourDesc = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
            TEnum.TournamentDyscypline enumType = (TEnum.TournamentDyscypline)int.Parse(tourDesc["Dyscypline"]);
            ITournament t = new Tournament(tourDesc["Name"], enumType);
            //team dictionary contains teams with id key
            Dictionary<int, TTeam.ITeam> teamDic = null;            
            //referees dictionary contains referees with id key
            Dictionary<int, TPerson.Referee> refDic = null;
            
            try
            {
                refDic = Referee(path);
                foreach (var x in refDic)
                {
                    t.AddReferee(x.Value);
                }
            }
            catch (TException.FileIsEmpty e)
            { }
            catch (Exception e)
            { }

            try
            {
                teamDic = Team(path, enumType);
                foreach (var x in teamDic)
                {
                    t.AddTeam(x.Value);
                }
            }
            catch (TException.FileIsEmpty e)
            { }
            catch (Exception e)
            { }

            try
            {
                t.League = League(path, refDic, teamDic, enumType);
                t.League.Teams = t.Teams;
                t.League.Referees = t.Referees;
            }
            catch (TException.FileIsEmpty e)
            { }
            catch (Exception e)
            { }

            try
            {
                t.PlayOff = PlayOff(path, refDic, teamDic, enumType);
            }
            catch(TException.FileIsEmpty e)
            { }
            catch(Exception e)
            { }

            return t;
        }
        /// <summary>
        /// create playoff object if exist
        /// </summary>
        /// <param name="path">path to tournamne tdirectory</param>
        /// <param name="referees">list of refeeres with is as key</param>
        /// <param name="teams">list of teams with is as key</param>
        /// <param name="tenum">tournament dyscypline enum</param>
        /// <returns> playoff object</returns>
        public static TRound.PlayOff PlayOff(string path, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams, TEnum.TournamentDyscypline tenum)
        {
            TRound.PlayOff p = new TRound.PlayOff();
            if (File.ReadLines(path + "\\playoff.json").First() == "null") 
                throw new TException.FileIsEmpty();
            var json = JsonConvert.DeserializeObject<Dictionary<string, List<RoundTempl>>>(File.ReadAllText(path + "\\playoff.json"))["Rounds"];
            List<TRound.Round> rP = new List<TRound.Round>();
            RoundTempl elem;

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

        /// <summary>
        /// create league object if exist
        /// </summary>
        /// <param name="path">path to tournamne directory</param>
        /// <param name="referees">list of refeeres with is as key</param>
        /// <param name="teams">list of teams with is as key</param>
        /// <param name="tenum">tournament dyscypline enum</param>
        /// <returns> league object</returns>
        public static TRound.League League(string path, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams, TEnum.TournamentDyscypline tenum)
        {
            TRound.League l =  new TRound.League();
            if (File.ReadLines(path + "\\league.json").First() == "null") throw new TException.FileIsEmpty();
            var json = JsonConvert.DeserializeObject<Dictionary<string, List<RoundTempl>>>(File.ReadAllText(path + "\\league.json"))["Rounds"];

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

        /// <summary>
        /// create round object
        /// </summary>
        /// <param name="elem">contains required element for round</param>
        /// <param name="tenum">tournament dyscypline as enum</param>
        /// <param name="referees">dictionary of referees with id key</param>
        /// <param name="teams">dictionary of teams with id key</param>
        /// <returns>round object</returns>
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

        /// <summary>
        /// create volleyball match object
        /// </summary>
        /// <param name="elem">contains required element for round</param>
        /// <param name="tenum">tournament dyscypline as enum</param>
        /// <param name="referees">dictionary of referees with id key</param>
        /// <param name="teams">dictionary of teams with id key</param>
        /// <returns>match object</returns>
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
            match.IsWalkover = elem.IsWalkover;

            return match;
        }

        /// <summary>
        /// create match object
        /// </summary>
        /// <param name="elem">contains required element for round</param>
        /// <param name="tenum">tournament dyscypline as enum</param>
        /// <param name="referees">dictionary of referees with id key</param>
        /// <param name="teams">dictionary of teams with id key</param>
        /// <returns>match object</returns>
        private static TMatch.Match CreateMatch(MatchTempl elem, TEnum.TournamentDyscypline tenum, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams)
        {
            TMatch.Match match;
            switch (tenum) {
                case TEnum.TournamentDyscypline.dodgeball:
                    
                     match = new TMatch.DodgeballMatch(teams[elem.TeamA], teams[elem.TeamB], new List<TPerson.Referee>{
                        referees[elem.RefA]
                    });
                break;
                case TEnum.TournamentDyscypline.tugofwar:

                    match = new TMatch.TugOfWarMatch(teams[elem.TeamA], teams[elem.TeamB], new List<TPerson.Referee>{
                        referees[elem.RefA]
                    });
                    break;
                default: return null;
            }

            if (elem.Winner != null)
            {
                match.Winner = teams[int.Parse(elem.Winner)];

            }
            match.IsWalkover = elem.IsWalkover;
            return match;
        }

        /// <summary>
        /// create dictionary of referees with id as a key
        /// </summary>
        /// <param name="path">path to tournament directory</param>
        /// <returns>dictionary of referees with id as a key object</returns>
        public static Dictionary<int, TPerson.Referee> Referee(string path)
        {
            if (File.ReadLines(path + "\\referees.json").First() == "null") throw new TException.FileIsEmpty();
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

        /// <summary>
        /// create dictionary of teams with id as a key
        /// </summary>
        /// <param name="path">path to tournament directory</param>
        /// <returns>dictionary of teams with id as a key object</returns>
        public static Dictionary<int, TTeam.ITeam> Team(string path, TEnum.TournamentDyscypline type)
        {
            if (File.ReadLines(path + "\\teams.json").First() == "null") throw new TException.FileIsEmpty();
            var str = File.ReadAllText(path + "\\teams.json");
            List<TeamTempl> teamDesc;


            teamDesc = JsonConvert.DeserializeObject<List<TeamTempl>>(str );
            
            List<TTeam.ITeam> team = new List<TTeam.ITeam>();
            Dictionary<int, TTeam.ITeam> teamDic = new Dictionary<int, TTeam.ITeam>();

            Dictionary<string, float> stats ;

            switch (type) {
                case TEnum.TournamentDyscypline.volleyball:
                    for (int i = 0; i < teamDesc.Count; i++)
                    {
                        stats = GetStats(teamDesc[i]);
                        teamDic.Add(teamDesc[i].Id, new TTeam.VolleyballTeam(teamDesc[i].Name,teamDesc[i].Id, Players(teamDesc[i].listPlayers),stats));
                    }
                    break;
                case TEnum.TournamentDyscypline.dodgeball:
                    for (int i = 0; i < teamDesc.Count; i++)
                    {
                        stats = GetStats(teamDesc[i]);
                        teamDic.Add(teamDesc[i].Id, new TTeam.DodgeballTeam(teamDesc[i].Name, teamDesc[i].Id, Players(teamDesc[i].listPlayers),stats));
                    }
                    break;
                case TEnum.TournamentDyscypline.tugofwar:
                    for (int i = 0; i < teamDesc.Count; i++)
                    {
                        stats = GetStats(teamDesc[i]);
                        teamDic.Add(teamDesc[i].Id, new TTeam.TugOfWarTeam(teamDesc[i].Name, teamDesc[i].Id, Players(teamDesc[i].listPlayers),stats));
                    }
                    break;

                default:
                    throw new TException.TournamentDyscyplineNotIdentify();
            }
            return teamDic;
        }
        
        /// <summary>
        /// set dictionary with stats to load to team
        /// </summary>
        /// <param name="t">stats that was read from file as teamTempl type</param>
        /// <returns>dictionary of stats</returns>
        private static Dictionary<string, float> GetStats(TeamTempl t)
        {
            Dictionary<string, float> stats = new Dictionary<string, float>();
            stats.Add("Points", t.Points );
            stats.Add("PlayersEliminated", t.PlayersEliminated );
            stats.Add("SumOfPlayersLeft", t.SumOfPlayersLeft );
            stats.Add("AvWinTime", t.AvWinTime );
            stats.Add("AvLossTime", t.AvLossTime );
            stats.Add("SumWinTime", t.SumWinTime );
            stats.Add("SumLossTime", t.SumLossTime );
            stats.Add("MatchesPlayed", t.MatchesPlayed );
            stats.Add("MatchesWon", t.MatchesWon );
            stats.Add("ScoreDiff",  t.ScoreDiff );

            return stats;
        }

        /// <summary>
        /// create team player
        /// </summary>
        /// <param name="playerDesc"> properties of player</param>
        /// <returns>player object</returns>
        private static List<TPerson.Player> Players(List<TeamTempl.Player> playerDesc)
        {
            
            List<TPerson.Player> players = new List<TPerson.Player>();
            
                    for (int i = 0; i < playerDesc.Count; i++)
                    {
                        players.Add( new TPerson.Player(playerDesc[i].Firstname, playerDesc[i].Lastname, playerDesc[i].Age, playerDesc[i].Number));
                    }
            return players;
        }
        
        /// <summary>
        /// get path to tournament directory
        /// </summary>
        /// <param name="name">tournament name</param>
        /// <returns>string with path</returns>
        private static string Path(string name)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\" + name;
        }



        // template classes using to deserialize from file  

        private class TeamTempl
        {
            public List<Player> listPlayers=null;
            public float Points = 0;
            public float ScoreDiff = 0;
            public int Id = 0;
            public string Name = null;
            public float MatchesPlayed = 0;
            public float MatchesWon = 0;
            public bool DidWithdraw = false;
            public float PlayersEliminated = 0;
            public float SumOfPlayersLeft = 0;
            public float AvWinTime = 0;
            public float AvLossTime = 0;
            public float SumWinTime = 0;
            public float SumLossTime = 0;

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
            public bool IsWalkover = false;


        }
    }
}
