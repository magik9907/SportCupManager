using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using TournamentManager.TPerson;
using TournamentManager.TTeam;

namespace TournamentManager
{
    namespace TRound
    {
        public class PlayOff
        {
            public PlayOff(List<TTeam.ITeam> t, List<TPerson.Referee> referees)
            {

            }
        }

        public class League
        {
            private List<Round> rounds;
            private List<TTeam.ITeam> teams = new List<TTeam.ITeam>();
            private List<TPerson.Referee> referees = new List<TPerson.Referee>();


            public League(List<TTeam.ITeam> t, List<TPerson.Referee> referees)
            {
                if (t.Count % 2 == 0)
                    rounds = new List<Round>(t.Count - 1);
                else
                    rounds = new List<Round>(t.Count);
                for(int i = 0; i < t.Count; i++)
                {
                    for(int j = 1; j + i < t.Count; j++)
                    {
                        if (t[i] == t[j + i])
                            throw new DuplicateTeamException(t[i]);
                    }
                }
                this.referees = referees;
            }
        }

        //Exception if there were two exactly same teams in the team list
        public class DuplicateTeamException : Exception
        {
            private TTeam.Team team;
            public DuplicateTeamException(TTeam.ITeam team)
            {
                this.team = (Team)team;
            }
            public override string Message
            {
                get
                {
                    return "Team" + team.Name + "is already playing in the league";
                }
            }
        }

        public class Round
        {
            private List<TMatch.Match> listMatches = new List<TMatch.Match>();
            private int[] date = new int[3];
            private string roundName;
            public Round(string name, int[] date)
            {
                //maxDays stores number of days in the month
                int maxDays;
                if (date.Length != 3)
                    throw new WrongDateFormatException();
                if(date[1] > 12 || date[1] <= 0)
                    throw new WrongMonthException();
                if (date[1] == 2)
                    if ((date[2] % 4 == 0 && date[2] % 100 != 0) || date[2] % 400 == 0)
                        maxDays = 29;
                    else
                        maxDays = 28;
                else
                    if ((date[1] < 8 && date[1] % 2 == 1) || (date[1] >= 8 && date[1] % 2 == 0))
                        maxDays = 31;
                    else
                        maxDays = 30;
                if (date[0] > maxDays || date[0] <= 0)
                    throw new WrongDayException();
                this.date = date;
                roundName = name;
            }

            public string getRoundName()
            {
                return roundName;
            }

            public void addMatch(TMatch.Match match, List<Referee> referees)
            {
                match.setReferees(referees);
                //this checks whether or not one of the teams specified are already playing someone this round
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (listMatches[i].isPlaying(match.getTeamA()))
                        throw new AlreadyPlayingException(match, match.getTeamA());
                    if (listMatches[i].isPlaying(match.getTeamB()))
                        throw new AlreadyPlayingException(match, match.getTeamB());
                }
                listMatches.Add(match);
            }

            //check whether or not a team has played in this round
            public Boolean isPlaying(TTeam.ITeam team)
            {
                Boolean flag = false;
                for (int i = 0; i < listMatches.Count; i++)
                {
                    flag = flag || listMatches[i].isPlaying(team);
                }
                return flag;
            }
        }

        //Exception if team has already a match scheduled in this round
        public class AlreadyPlayingException : Exception
        {
            private TMatch.Match match;
            private TTeam.ITeam team;
            public AlreadyPlayingException(TMatch.Match match, TTeam.ITeam team)
            {
                this.match = match;
                this.team = team;
            }
            public override string Message
            {
                get
                {
                    TTeam.ITeam team2;
                    if (team == match.getTeamA())
                        team2 = match.getTeamB();
                    else
                        team2 = match.getTeamA();
                    return team + " is already scheduled to play this round against " + team2;
                }
            }
        }

        //Exception if day doesn't fit in number of days in a specified month
        public class WrongDayException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Day number is out of expected range";
                }
            }
        }

        //Exception if month sent doesn't match number of months
        public class WrongMonthException : Exception
        {
            public override string Message
            {
                get 
                {
                    return "Month number is out of expected range";
                }
            }
        }

        //Exception if the table didn't have 3 fields for day, month and year
        public class WrongDateFormatException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Date is in the wrong format";
                }
            }
        }
    }
}
