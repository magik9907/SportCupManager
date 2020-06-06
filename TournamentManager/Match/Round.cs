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
                //If teams number is even we need one less round than teams
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
            public void ScheduleMatch(TMatch.Match match, List<Referee> referees, Round round)
            {
                Boolean flagA;
                Boolean flagB;
                Boolean flagAnB;
                for (int i = 0; i < teams.Count; i++)
                {
                    flagA = false;
                    flagB = false;
                    flagAnB = false;
                    for (int j = 0; j < rounds.Count; j++)
                    {
                        /*
                         * We have to check 3 things for each team:
                         * 1. Is that team one of the teams participating in the match
                         * 2. If not, has this team already played against both teams from match
                         * 3. If not, can this team play the teams he has not played with in other round(s)
                         * 
                         * Additionally, for both teams participating in the match we have to check n things
                         * 1. Have they already played against each other
                         * 2. Are they already playing against someone else in that round (checked by round)
                         */
                        if (rounds[j].IsScheduled(match.TeamA, match.TeamB))
                            throw new AlreadyPlayingInLeagueException(match);
                        if (!match.isPlaying(teams[i]) && rounds[j] != round)
                        {
                            if (rounds[j].IsScheduled(teams[i], match.TeamA))
                                flagA = true;
                            if (rounds[j].IsScheduled(teams[i], match.TeamB))
                                flagB = true;
                            if (!rounds[j].IsPlaying(match.TeamA) && !rounds[j].IsPlaying(teams[i]))
                                if (!rounds[j].IsPlaying(match.TeamB) && !rounds[j].IsPlaying(teams[i]) && flagAnB == false)
                                    flagAnB = true;
                                else
                                    flagA = true;
                            else
                                if (!rounds[j].IsPlaying(match.TeamB) && !rounds[j].IsPlaying(teams[i]))
                                    flagB = true;
                        }
                    }
                    if (!(flagA || flagB) || !(flagAnB || (flagA && flagB)))
                        throw new ImpossibleScheduleException();
                }
                round.AddMatch(match, referees);
            }
        }

        //Exception if the schedule is impossible
        public class ImpossibleScheduleException : Exception
        {
            public override string Message
            {
                get
                {
                    return "This schedule is impossible";
                }
            }
        }

        //Exception if the teams have already a match scheduled in the league
        public class AlreadyPlayingInLeagueException : Exception
        {
            private TMatch.Match match;
            public AlreadyPlayingInLeagueException(TMatch.Match match)
            {
                this.match = match;
            }
            public override string Message
            {
                get
                {
                    return match.TeamA + " has already a match scheduled with " + match.TeamB;
                }
            }
        }

        //Exception if there were two exactly same teams in the team list
        public class DuplicateTeamException : Exception
        {
            private TTeam.ITeam team;
            public DuplicateTeamException(TTeam.ITeam team)
            {
                this.team = team;
            }
            public override string Message
            {
                get
                {
                    return "Team" + team + "is already playing in the league";
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

            public string RoundName
            {
                get{ return roundName; }
            }

            public void AddMatch(TMatch.Match match, List<Referee> referees)
            {
                match.SetReferees(referees);
                //this checks whether or not one of the teams specified are already playing someone this round
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (listMatches[i].isPlaying(match.TeamA))
                        throw new AlreadyPlayingInRoundException(match, match.TeamA);
                    if (listMatches[i].isPlaying(match.TeamB))
                        throw new AlreadyPlayingInRoundException(match, match.TeamB);
                }
                listMatches.Add(match);
            }

            //check whether or not a team has played in this round
            public Boolean IsPlaying(TTeam.ITeam team)
            {
                Boolean flag = false;
                for (int i = 0; i < listMatches.Count; i++)
                {
                    flag = flag || listMatches[i].isPlaying(team);
                }
                return flag;
            }

            public Boolean IsScheduled(TTeam.ITeam team1, TTeam.ITeam team2)
            {
                for (int i = 0; i < listMatches.Count; i++)
                    if (listMatches[i].isPlaying(team1) && listMatches[i].isPlaying(team2))
                        return true;
                return false;
            }
        }

        //Exception if team has already a match scheduled in this round
        public class AlreadyPlayingInRoundException : Exception
        {
            private TMatch.Match match;
            private TTeam.ITeam team;
            public AlreadyPlayingInRoundException(TMatch.Match match, TTeam.ITeam team)
            {
                this.match = match;
                this.team = team;
            }
            public override string Message
            {
                get
                {
                    TTeam.ITeam team2;
                    if (team == match.TeamA)
                        team2 = match.TeamB;
                    else
                        team2 = match.TeamA;
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

        //Exception if the table didn't have exactly 3 fields (for day, month and year)
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
