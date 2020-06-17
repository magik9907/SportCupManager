using System;
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
        public class Round
        {
            public List<TMatch.Match> listMatches = new List<TMatch.Match>();
            private int[] date = new int[3];
            private string roundName;
            public int[] Date
            { get { return date; } }
            public string RoundName
            {
                get { return roundName; }
            }
            public List<TMatch.Match> ListMatches
            {
                get { return listMatches; }
            }
            public Round(string name, int[] date)
            {
                if (date.Length != 3)
                    throw new WrongDateFormatException(date);
                if (date[1] > 12 || date[1] <= 0)
                    throw new WrongMonthException(date);
                if (date[0] > MaxDays(date) || date[0] <= 0)
                    throw new WrongDayException(date);
                this.date = date;
                roundName = name;
            }
            public Round(Round round)
            {
                this.date = round.date;
                this.listMatches = round.listMatches;
                this.roundName = round.roundName;
            }
            public Round CreateCopy()
            {
                return new Round(this);
            }
            public void AddMatch(TMatch.Match match)
            {
                //this checks whether or not one of the teams specified are already playing someone this round
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (listMatches[i].IsPlaying(match.TeamA))
                        throw new AlreadyPlayingInRoundException(CreateCopy(), match, match.TeamA);
                    if (listMatches[i].IsPlaying(match.TeamB))
                        throw new AlreadyPlayingInRoundException(CreateCopy(), match, match.TeamB);
                }
                listMatches.Add(match);
            }

            //returns a match played in the round by a specified team
            public TMatch.Match GetMatch(TTeam.ITeam team)
            {
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (listMatches[i].IsPlaying(team))
                        return listMatches[i];
                }
                throw new TeamNotPlayingException(CreateCopy(), team);
            }

            //check whether or not a team has played in this round
            public Boolean IsPlaying(TTeam.ITeam team)
            {
                Boolean flag = false;
                for (int i = 0; i < listMatches.Count; i++)
                {
                    flag = flag || listMatches[i].IsPlaying(team);
                }
                return flag;
            }
            //check whether or not two teams are playing against each other
            public Boolean IsScheduled(TTeam.ITeam team1, TTeam.ITeam team2)
            {
                for (int i = 0; i < listMatches.Count; i++)
                    if (listMatches[i].IsPlaying(team1) && listMatches[i].IsPlaying(team2))
                        return true;
                return false;
            }
            //check whether or not all of the matches in this round have been completed
            public Boolean IsFinished()
            {
                if (listMatches.Count == 0)
                    return false;
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (!listMatches[i].WasPlayed())
                        return false;
                }
                return true;
            }
            internal void SetResult(string stat, TTeam.ITeam winner)
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
    }
}
