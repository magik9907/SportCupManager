using System;
using System.Collections.Generic;
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
            public League(List<TTeam.ITeam> t, List<TPerson.Referee> referees)
            {

            }
        }

        public class Round
        {
            private List<TMatch.Match> listMatches = new List<TMatch.Match>();
            private int[] date = new int[3];
            private string roundName;
            public Round(string name, int[] date)
            {
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
                for (int i = 0; i < listMatches.Count; i++)
                {
                    if (match.getTeamA() == listMatches[i].getTeamA() || match.getTeamA() == listMatches[i].getTeamB())
                        throw new AlreadyPlayingException(match, match.getTeamA());
                    if (match.getTeamB() == listMatches[i].getTeamA() || match.getTeamB() == listMatches[i].getTeamB())
                        throw new AlreadyPlayingException(match, match.getTeamB());
                }
                listMatches.Add(match);
            }
        }

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
