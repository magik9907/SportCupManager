﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using TournamentManager.TRound;
using System.Runtime.Serialization;

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

            t.League = League(path, refDic, teamDic);


            return t;
        }


        public static TRound.League League(string path, Dictionary<int, TPerson.Referee> referees, Dictionary<int, TTeam.ITeam> teams)
        {
            TRound.League l = new TRound.League();

            var json = (JsonConvert.DeserializeObject<Dictionary<string, List<RoundTempl>>>(File.ReadAllText(path + "\\league.json")))["Rounds"];



            return l;
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

                case TEnum.TournamentDyscypline.dodgeball:
                    for (int i = 0; i < teamDesc.Count; i++)
                    {
                        teamDic.Add(teamDesc[i].Id, new TTeam.DodgeballTeam(teamDesc[i].Name, teamDesc[i].Id, Players(teamDesc[i].listPlayers)));
                    }
                    break;

                case TEnum.TournamentDyscypline.tugofwar:
                    for (int i = 0; i < teamDesc.Count; i++)
                    {
                        teamDic.Add(teamDesc[i].Id, new TTeam.TugOfWarTeam(teamDesc[i].Name, teamDesc[i].Id, Players(teamDesc[i].listPlayers)));
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

        private class RoundTempl
        {
            public string RoundName;
            public List<MatchTempl> ListMatches;
            
        }
        private class MatchTempl
        {
            public int[] assistantReferees;

            public int TeamA;
            public int TeamB;
            public int Winner;
            public int RefA;


        }
    }
}
