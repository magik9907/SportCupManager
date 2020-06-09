using System;
using System.Collections.Generic;
using System.Linq;
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
            private List<Round> rounds = new List<Round>(2);
            private List<TPerson.Referee> referees;
            public PlayOff(List<TTeam.ITeam> t, List<TPerson.Referee> referees, int[] startDate)
            {
                //semis and finals will be generated on two consecutive days
                rounds[0] = new Round("semi-finals", startDate);
                try
                {
                    startDate[0]++;
                    rounds[1] = new Round("final", startDate);
                }
                catch (WrongDayException)
                {
                    startDate[0] = 1;
                    if (startDate[1] == 12)
                    {
                        startDate[1] = 1;
                        startDate[2]++;
                    }
                    else
                        startDate[1]++;
                    rounds[1] = new Round("final", startDate);
                }
                GenerateRound(t, "semi-finals");
                this.referees = referees;
            }
            public TTeam.ITeam GetWinner()
            {
                return rounds[1].ListMatches[0].Winner;
            }
            private void GenerateRound(List<TTeam.ITeam> teams, string name)
            {
                int roundNumber, refNumber = 1;
                if (name == "final")
                    roundNumber = 1;
                else
                    roundNumber = 0;
                if (teams[0] is VolleyballTeam)
                    refNumber = 3;
                for (int i = 0; i < teams.Count / 2; i++)
                    rounds[roundNumber].AddMatch(TMatch.Match.CreateMatch(teams[i], teams[teams.Count - 1 - i], referees.GetRange(i*refNumber, refNumber)), referees.GetRange(i*refNumber, refNumber));
            }
            public void SetResult(string stat, TTeam.ITeam winner, int roundNumber)
            {
                rounds[roundNumber].SetResult(stat, winner);
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

            private void sortTeams()
            {
                for(int i = 0; i < teams.Count; i++)
                    for(int j = 0; j < teams.Count-1; j++)
                    {
                        if(teams[0] is VolleyballTeam)
                            if((VolleyballTeam)teams[j] > (VolleyballTeam)teams[j+1])
                            {
                                var tmp = teams[j];
                                teams[j] = teams[j + 1];
                                teams[j + 1] = tmp;
                            }
                        if (teams[0] is DodgeballTeam)
                            if ((DodgeballTeam)teams[j] > (DodgeballTeam)teams[j + 1])
                            {
                                var tmp = teams[j];
                                teams[j] = teams[j + 1];
                                teams[j + 1] = tmp;
                            }
                        if (teams[0] is TugOfWarTeam)
                            if ((TugOfWarTeam)teams[j] > (TugOfWarTeam)teams[j + 1])
                            {
                                var tmp = teams[j];
                                teams[j] = teams[j + 1];
                                teams[j + 1] = tmp;
                            }
                    }
            }
            public List<TTeam.ITeam> getFinalTeams(int number)
            {
                return teams.GetRange(0, number);
            }
            //this is for manual scheduling. Probably should make a flag to make it exclusive with 
            public void ScheduleMatch(TMatch.Match match, List<Referee> referees, Round round)
            {
                for (int i = 0; i < teams.Count; i++)
                {
                    Boolean flagA = false;
                    Boolean flagB = false;
                    Boolean flagAnB = false;
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
            //this is for automatic scheduling
            public void AutoSchedule(int[] startDate, int spaceBetweenMatches)
            {
                //refNumber holds the number of referees required for a match
                int i = 1, j = 1, refNumber = 1;
                if (teams[0] is VolleyballTeam)
                    refNumber = 3;
                //rounds.Count/2 + 1 is the maximum number of matches played in a round
                for(int index = 0; index < rounds.Count * (rounds.Count/2 + 1); index++)
                {
                    if (index % (rounds.Count/2 + 1) == 0)
                    {
                        rounds[index / (rounds.Count / 2 + 1)] = new Round(index / (rounds.Count / 2 + 1) + " Round", startDate);
                        startDate = IncrementDate(startDate, spaceBetweenMatches);
                        j = i;
                    }
                    if (i != j)
                        rounds[index / (rounds.Count / 2 + 1)].AddMatch(TMatch.Match.CreateMatch(teams[i], teams[j], referees.GetRange((index % (rounds.Count / 2 + 1) - 1) * refNumber, refNumber)), referees.GetRange((index % (rounds.Count / 2 + 1) - 1) * refNumber, refNumber));
                    else
                        if (teams.Count != rounds.Count)
                            rounds[index / (rounds.Count / 2 + 1)].AddMatch(TMatch.Match.CreateMatch(teams[i], teams[^1], referees.GetRange(referees.Count - refNumber, refNumber)), referees.GetRange(referees.Count - refNumber, refNumber));
                    i = ++i % rounds.Count;
                    j = --j % rounds.Count;
                }
            }
            //increment date changes date by a specified number of days
            private int[] IncrementDate(int[] date, int increment)
            {
                date[0] += increment;
                while(date[0] > Round.MaxDays(date))
                {
                    date[0] -= Round.MaxDays(date);
                    if (date[1]++ > 12)
                    {
                        date[1] = 1;
                        date[2]++; 
                    }
                }
                return date;
            }
        }

        //Exception if the schedule is impossible
        public class ImpossibleScheduleException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Can't schedule this match as it would make scheduling rest of the season impossible";
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
                    return match.TeamA.Name + " has already a match scheduled with " + match.TeamB.Name;
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
                    return "Team " + team.Name + " is already playing in the league";
                }
            }
        }

        public class Round
        {
            private List<TMatch.Match> listMatches = new List<TMatch.Match>();
            private int[] date = new int[3];
            private string roundName;
            public List<TMatch.Match> ListMatches
            {
                get { return listMatches; }
            }
            public Round(string name, int[] date)
            {
                if (date.Length != 3)
                    throw new WrongDateFormatException();
                if (date[1] > 12 || date[1] <= 0)
                    throw new WrongMonthException();
                if (date[0] > MaxDays(date) || date[0] <= 0)
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

            //returns a match played in the round by a specified team
            public TMatch.Match GetMatch(TTeam.ITeam team)
            {
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (listMatches[i].isPlaying(team))
                        return listMatches[i];
                }
                throw new TeamNotPlayingException(team);
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
            //check whether or not two teams are playing against each other
            public Boolean IsScheduled(TTeam.ITeam team1, TTeam.ITeam team2)
            {
                for (int i = 0; i < listMatches.Count; i++)
                    if (listMatches[i].isPlaying(team1) && listMatches[i].isPlaying(team2))
                        return true;
                return false;
            }
            //check whether or not all of the matches in this round have been completed
            public Boolean IsFinished()
            {
                for(int i = 0; i < listMatches.Count; i++)
                {
                    if (!listMatches[i].WasPlayed())
                        return false;
                }
                return true;
            }

            public void SetResult(string stat, TTeam.ITeam winner)
            {
                GetMatch(winner).SetResult(stat, winner);
            }

            //returns number of days in month and year held in date
            internal static int MaxDays(int[] date)
            {
                if (date[1] == 2)
                    if ((date[2] % 4 == 0 && date[2] % 100 != 0) || date[2] % 400 == 0)
                        return 29;
                    else
                        return 28;
                else
                    if ((date[1] < 8 && date[1] % 2 == 1) || (date[1] >= 8 && date[1] % 2 == 0))
                        return 31;
                    else
                        return 30;
            }
        }

        //Exception if team is not playing in the round
        public class TeamNotPlayingException : Exception
        {
            private TTeam.ITeam team;
            public TeamNotPlayingException(TTeam.ITeam team)
            {
                this.team = team;
            }
            public override string Message
            {
                get
                {
                    return team.Name + " is not playing in this round";
                }
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
                    if (team == match.TeamA)
                        return team.Name + " is already scheduled to play this round against " + match.TeamB.Name;
                    else
                        return team.Name + " is already scheduled to play this round against " + match.TeamA.Name;
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
