﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using TournamentManager.TException;
using TournamentManager.TPerson;
using TournamentManager.TTeam;
using Newtonsoft.Json;
using TournamentManager.TMatch;

namespace TournamentManager
{
    namespace TRound
    {
        public class League
        {
            private List<Round> rounds;
            [JsonProperty]
            public List<Round> Rounds
            {
                get { return rounds; }
                set { rounds = value; }
            }
            [JsonIgnore]
            public List<TTeam.ITeam> Teams
            {
                get 
                {
                    return teams; 
                }
                set
                {
                    teams = value;
                }
            }
            private List<TTeam.ITeam> teams = new List<TTeam.ITeam>();
            private List<TPerson.Referee> referees = new List<TPerson.Referee>();
            public List<TPerson.Referee> Referees
            {
                set
                {
                    referees = value;
                }
            }
            //generate league object 
            //is used to read from file in Read class
            //DON'T REMOVE!!!
            public League() { }
            public League(List<TTeam.ITeam> t, List<TPerson.Referee> referees)
            {
                int refsRequired = 1;
                if (t[0] is VolleyballTeam)
                    refsRequired = 3;
                //If teams number is even we need one less round than teams
                if (t.Count % 2 == 0)
                    rounds = new List<Round>(t.Count - 1);
                else
                    rounds = new List<Round>(t.Count);
                //this part checks whether or not there are any duplicate teams on the list
                for (int i = 0; i < t.Count; i++)
                    for (int j = 1; j + i < t.Count; j++)
                        if (t[i] == t[j + i])
                            throw new DuplicateTeamException(t[i]);
                if (referees.Count < (t.Count / 2) * refsRequired)
                    throw new NotEnoughRefereesException(referees.Count, t.Count / 2 * refsRequired);
                else
                    this.referees = referees;
                this.teams = t;
            }
            public League(League copy)
            {
                this.teams = copy.teams;
                this.rounds = copy.rounds;
                this.referees = copy.referees;
            }
            public League CreateCopy()
            {
                return new League(this);
            }
            public void SortTeams()
            {
                for (int i = 0; i < teams.Count; i++)
                    for (int j = 0; j < teams.Count - 1; j++)
                    {
                        if (teams[0] is VolleyballTeam)
                            if ((VolleyballTeam)teams[j] < (VolleyballTeam)teams[j + 1])
                            {
                                var tmp = teams[j];
                                teams[j] = teams[j + 1];
                                teams[j + 1] = tmp;
                            }
                        if (teams[0] is DodgeballTeam)
                            if ((DodgeballTeam)teams[j] < (DodgeballTeam)teams[j + 1])
                            {
                                var tmp = teams[j];
                                teams[j] = teams[j + 1];
                                teams[j + 1] = tmp;
                            }
                        if (teams[0] is TugOfWarTeam)
                            if ((TugOfWarTeam)teams[j] < (TugOfWarTeam)teams[j + 1])
                            {
                                var tmp = teams[j];
                                teams[j] = teams[j + 1];
                                teams[j + 1] = tmp;
                            }
                    }
            }
            public void SetResult(string stat, TTeam.ITeam winner, TTeam.ITeam loser)
            {
                for (int i = 0; i < rounds.Count; i++)
                    if (rounds[i].IsScheduled(winner, loser))
                    {
                        rounds[i].SetResult(stat, winner);
                        break;
                    }
            }
            public List<TTeam.ITeam> GetFinalTeams(int number)
            {
                SortTeams();
                return teams.GetRange(0, number);
            }

            //this is for manual scheduling. Probably should make a flag to make it exclusive with 
            public void ScheduleMatch(TMatch.Match match, int[] date)
            {
                for (int i = 0; i < teams.Count; i++)
                {
                    Boolean flagA = false;
                    Boolean flagB = false;
                    Boolean flagAnB = false;
                    if (rounds.Capacity >= rounds.Count + 1)
                        flagAnB = true;
                    if (rounds.Capacity - rounds.Count >= 2)
                        flagA = true;
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
                            throw new AlreadyPlayingInLeagueException(CreateCopy(), match);
                        if (!match.IsPlaying(teams[i]))
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
                    if (((!flagAnB && !(flagA && flagB)) || !(flagA || flagB)) && !match.IsPlaying(teams[i]))
                        throw new ImpossibleScheduleException(CreateCopy());
                }
                for (int i = 0; i < rounds.Count; i++)
                    if (rounds[i].Date[0] == date[0] && rounds[i].Date[1] == date[1] && rounds[i].Date[2] == date[2])
                    {
                        try
                        {
                            rounds[i].AddMatch(match);
                        }
                        catch (RoundRuntimeException e)
                        {
                            rounds.Add(e.RecreateRound());
                            throw new ImpossibleScheduleException(CreateCopy());
                        }
                        return;
                    }
                try
                {
                    Round tmp = new Round("round played on " + date[0] + "/" + date[1] + "/" + date[2], date);
                    tmp.AddMatch(match);
                    rounds.Add(tmp);
                }
                catch (DateException)
                {
                    throw new ImpossibleScheduleException(CreateCopy());
                }
            }
            //this is for automatic scheduling
            public void AutoSchedule(int[] startDate, int spaceBetweenMatches)
            {
                //refNumber holds the number of referees required for a match
                int i = 0, j = 0, refNumber = 1;
                if (teams[0] is VolleyballTeam)
                    refNumber = 3;
                //rounds.Capacity/2 + 1 is the maximum number of matches played in a round
                for (int index = 0; index < rounds.Capacity; index++)
                {
                    Round tmp = new Round("Round " + (index + 1), startDate);
                    startDate = IncrementDate(startDate, spaceBetweenMatches);
                    for (int index2 = 0; index2 < rounds.Capacity / 2 + 1; index2++)
                    {
                        if (i != j)
                            tmp.AddMatch(TMatch.Match.CreateMatch(teams[i], teams[j], referees.GetRange((index2 - 1) * refNumber, refNumber)));
                        else
                        {
                            if (teams.Count != rounds.Capacity)
                                tmp.AddMatch(TMatch.Match.CreateMatch(teams[i], teams[^1], referees.GetRange(referees.Count - refNumber, refNumber)));
                        }
                        i = (i + 1) % rounds.Capacity;
                        j = ((j - 1) + rounds.Capacity) % rounds.Capacity;
                    }
                    rounds.Add(tmp);
                    j = i;
                }
            }

            //increment date changes date by a specified number of days
            private int[] IncrementDate(int[] date, int increment)
            {
                date[0] += increment;
                while (date[0] > Round.MaxDays(date))
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

            public bool IsFinished()
            {
                if(rounds.Count == rounds.Capacity)
                {
                    for (int i = 0; i < rounds.Count; i++)
                    {
                        if (!rounds[i].IsFinished())
                            return false;
                    }
                    return true;
                }
                return false;
            }
            public void WithdrawTeam(TTeam.ITeam t)
            {
                int teamsLeft = 0;
                for (int i = 0; i < teams.Count; i++)
                    if (!teams[i].DidWithdraw)
                        teamsLeft++;
                if (teamsLeft <= 4)
                    throw new NotEnoughTeamsLeftNumber(CreateCopy(), t);
                for (int i = 0; i < rounds.Count; i++)
                    if(rounds[i].IsFinished() && rounds[i].IsPlaying(t))
                            rounds[i].GetMatch(t).Walkover(t);
                t.Withdraw();
            }
        }
    }
}
