using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TournamentManager.TRound;

namespace TournamentManager
{
    namespace TException
    {
        public class TournamentDyscyplineNotIdentify : Exception
        {
            public TournamentDyscyplineNotIdentify() : base("unknown tournament dyscypline")
            {

            }
        }

        public class TournamentNameMustBeDefine : Exception
        {
            public TournamentNameMustBeDefine() : base("Tournament Name is not defined")
            {

            }
        }

        /// <summary>
        /// custom exception when no data given
        /// </summary>
        public class TournamentMustBeDefine : Exception
        {
            public TournamentMustBeDefine() : base("Name and type must be define")
            {

            }
        }
        /// <summary>
        /// custom exception of tournament data
        /// </summary>
        public class TournamentDataException : Exception
        {
            /// <summary>
            /// define what type of incorect data exception
            /// </summary>
            /// <param name="type">type of exception</param>
            public TournamentDataException(string type) : base(type + "must be define")
            {

            }


        }
        /// <summary>
        /// throw exception of not enought teams in tournament
        /// </summary>
        public class NotEnoughtTeamsNumber: Exception{
            
            /// <param name="number">current number of teams</param>
            public NotEnoughtTeamsNumber(int number):base("insufficient number of teams. Number of teams: "+number)
            { 
        
            }
        }
        /// <summary>
        /// exception of object dind't create
        /// </summary>
        public class ObjectNotDefined : Exception
        {
            /// <param name="TObject">type of set type</param>
            public ObjectNotDefined(string TObject) : base(TObject + " is not defined")
            {

            }
        }

        public class TeamMissingNameException : Exception
        {
            public TeamMissingNameException() : base("There is missing name parameter for Team.")
            {

            }
        }

        //set of Exceptions which may occur whlie creating an object
        public abstract class ObjectCreationException : Exception
        { }

        //set of Exceptions which may occur while operating on a Match
        public abstract class MatchRuntimeException : Exception
        {
            protected TMatch.Match match;
            public MatchRuntimeException(TMatch.Match match)
            {
                this.match = match;
            }
            public TMatch.Match RecreateMatch()
            {
                return match;
            }
        }

        //set of Exceptions which may occur while operating on Round
        public abstract class RoundRuntimeException : Exception
        {
            protected TRound.Round round;
            public RoundRuntimeException(TRound.Round round)
            {
                this.round = round;
            }
            public TRound.Round RecreateRound()
            {
                return round;
            }
        }

        //set of Exception related to dates
        public abstract class DateException : ObjectCreationException
        {
            protected int[] date;
            public DateException(int[] date)
            {
                this.date = date;
            }
        }

        //set of Exceptions which may occur while operating on League
        public abstract class LeagueRuntimeException : Exception
        {
            protected TRound.League league;
            public LeagueRuntimeException(TRound.League league)
            {
                this.league = league;
            }
            public TRound.League RecreateLeague()
            {
                return league;
            }
        }

        //set of Exceptions which may occur while operating on PlayOff(will try to finish soon)
        public abstract class PlayOffRuntimeException : Exception
        {
            protected TRound.PlayOff playoff;
            public PlayOffRuntimeException(TRound.PlayOff playoff)
            {
                this.playoff = playoff;
            }
            public TRound.PlayOff RecreatePlayOff()
            {
                return playoff;
            }
        }

        //Exception if PlayOff is finished but user tries to set a result
        public class AlreadyFinishedException : PlayOffRuntimeException
        {
            private TTeam.ITeam winner;
            public AlreadyFinishedException(TRound.PlayOff playoff, TTeam.ITeam winner) : base(playoff)
            {
                this.winner = winner;
            }
            public override string Message
            {
                get
                {
                    return "The tournament has already finished! " + winner.Name + " has won the Tournament";
                }
            }
        }

        //Exception if the number of referees is not enough to hold all of the games at once
        public class NotEnoughRefereesException : ObjectCreationException
        {
            int refsRequired, refsNumber;
            public NotEnoughRefereesException(int refsNumber, int refsRequired)
            {
                this.refsRequired = refsRequired;
                this.refsNumber = refsNumber;
            }
            public override string Message
            {
                get { return "Received amount of referees: " + refsNumber + " is too low. The required number is " + refsRequired; }
            }
        }

        //Exception if the schedule is impossible
        public class ImpossibleScheduleException : LeagueRuntimeException
        {
            public ImpossibleScheduleException(TRound.League league) : base(league)
            { }
            public override string Message
            {
                get
                {
                    return "Can't schedule this match as it would make scheduling rest of the season impossible";
                }
            }
        }

        //Exception if the teams have already a match scheduled in the league
        public class AlreadyPlayingInLeagueException : LeagueRuntimeException
        {
            private TMatch.Match match;
            public AlreadyPlayingInLeagueException(TRound.League league, TMatch.Match match) : base(league)
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

        //Exception if user wanted to withdraw a team from the league and there would be less than 4 teams left
        public class NotEnoughTeamsLeftNumber : LeagueRuntimeException
        {
            TTeam.ITeam team;
            public NotEnoughTeamsLeftNumber(TRound.League league, TTeam.ITeam team) : base(league)
            {
                this.team = team;
            }
            public override string Message
            {
                get
                {
                    return "Withdrawing " + team.Name + " would leave less than 4 teams in the league. If you want to withdraw " + team.Name + ", please set up PlayOff first.";
                }
            }
        }

        //Exception if user wanted to autoschedule after scheduling any matches manually
        public class ForbiddenAutoScheduleException : LeagueRuntimeException
        {
            public ForbiddenAutoScheduleException (TRound.League league) : base(league){}
            public override string Message
            {
                get
                {
                    return "You can't autoschedule after scheduling a match manually";
                }
            }
        }

        //Exception if there were two exactly same teams in the team list
        public class DuplicateTeamException : ObjectCreationException
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
                    return "Team " + team.Name + " has been registered twice";
                }
            }
        }

        //Exception if team is not playing in the round
        public class TeamNotPlayingException : RoundRuntimeException
        {
            private TTeam.ITeam team;
            public TeamNotPlayingException(TRound.Round round, TTeam.ITeam team) : base(round)
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
        public class AlreadyPlayingInRoundException : RoundRuntimeException
        {
            private TMatch.Match match;
            private TTeam.ITeam team;
            public AlreadyPlayingInRoundException(TRound.Round round, TMatch.Match match, TTeam.ITeam team) : base(round)
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
        public class WrongDayException : DateException
        {
            public WrongDayException(int[] date) : base(date)
            { }
            public override string Message
            {
                get
                {
                    return "Day number: " + date[0] + " is out of expected range";
                }
            }
        }

        //Exception if month sent doesn't match number of months
        public class WrongMonthException : DateException
        {
            public WrongMonthException(int[] date) : base(date)
            { }
            public override string Message
            {
                get
                {
                    return "Month number: " + date[1] + " is out of expected range";
                }
            }
        }

        //Exception if the table didn't have exactly 3 fields (for day, month and year)
        public class WrongDateFormatException : DateException
        {
            public WrongDateFormatException(int[] date) : base(date)
            { }
            public override string Message
            {
                get
                {
                    string tmp = "Date";
                    for (int i = 0; i < date.Length; i++)
                    {
                        if (i == 0)
                            tmp += tmp[0];
                        else
                            tmp += "/" + date[i];
                    }
                    return tmp + " is not in a supported format";
                }
            }
        }

        //Excpetion if teamA and teamB are the same team
        public class IncorrectOpponentException : ObjectCreationException
        {
            public override string Message
            {
                get
                {
                    return "A team cannot play against itself!";
                }
            }
        }

        //Exception if team set as winner is not playing in the match
        public class WinnerIsNotPlayingException : MatchRuntimeException
        {
            public WinnerIsNotPlayingException(TMatch.Match match) : base (match)
            { }
            public override string Message
            {
                get
                {
                    return "Team you want to set as a winner is not playing!";
                }
            }
        }

        //Exception if user wanted to set a result to a match that ended in walkover
        public class SetResultForWalkoverException : MatchRuntimeException
        {
            public SetResultForWalkoverException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "This match has ended by a walkover. You can't change this result";
                }
            }
        }

        //Exception if match length is not a number
        public class NotNumberMatchLengthException : MatchRuntimeException
        {
            public NotNumberMatchLengthException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Match length is supposed to be a number!";
                }
            }
        }

        //Exception if match length is negative
        public class NegativeMatchLengthException : MatchRuntimeException
        {
            public NegativeMatchLengthException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Match length is supposed to not be negative!";
                }
            }
        }

        //exception if number of players was not a whole number
        public class NotIntPlayersException : MatchRuntimeException
        {
            public NotIntPlayersException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Number of players should be a whole number!";
                }
            }
        }

        //exception if number of players was negative
        public class NegativePlayersNumberException : MatchRuntimeException
        {
            public NegativePlayersNumberException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Number of players should be positive!";
                }
            }
        }

        //Exception if number of players left was higher than players allowed on the field
        public class TooHighPlayersLeftException : MatchRuntimeException
        {
            public TooHighPlayersLeftException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Team can't have more players left than it started the game with (6)";
                }
            }
        }

        //Excpetion if name of a team passed to SetResult is not a name of any of the playing teams
        public class WrongNameInStatException : MatchRuntimeException
        {
            private string name;
            public WrongNameInStatException(TMatch.Match match, string name) : base(match)
            {
                this.name = name;
            }
            public override string Message
            {
                get { return name + " is not one of the teams playing in the match"; }
            }
        }

        //Exception if third set was played in spite of a team winning in two sets
        public class ThirdSetException : MatchRuntimeException
        {
            private TTeam.ITeam winner;
            public ThirdSetException(TMatch.Match match, TTeam.ITeam winner) : base(match)
            {
                this.winner = winner;
            }
            public override string Message
            {
                get
                {
                    return winner + " has won the match in two sets, third set should not be played";
                }
            }
        }

        //Exception if set was played but no team has won
        public class NoSetWinnerException : MatchRuntimeException
        {
            private int set;
            private int scoreRequired;
            public NoSetWinnerException(TMatch.Match match, int set) : base(match)
            {
                this.set = set;
                if (set != 3)
                    scoreRequired = 21;
                else
                    scoreRequired = 15;
            }
            public override string Message
            {
                get
                {
                    return "No team has won the set number " + set + " since no team has reached " + scoreRequired + " points while also having the lead";
                }
            }
        }

        //Exception if score was not an integer value
        public class NonIntScoreException : MatchRuntimeException
        {
            public NonIntScoreException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Score should be a whole number";
                }
            }
        }

        //Exception if score was negative
        public class NegativeScoreException : MatchRuntimeException
        {
            public NegativeScoreException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Score should be a positive number";
                }
            }
        }

        //Exception if a score entered was over the maximum points in a set
        public class TooHighScoreException : MatchRuntimeException
        {
            public TooHighScoreException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "No team can have more than 21 points in the first two sets and 15 in the last";
                }
            }
        }

        //Exception if string stat got separated into a different amount of strings than expected
        public class WrongStatFormatException : MatchRuntimeException
        {
            public WrongStatFormatException(TMatch.Match match) : base(match)
            { }
            public override string Message
            {
                get
                {
                    return "Format of string \"stat\" is incorrect!";
                }
            }
        }

        //Exception if the team that was set as winner lost based on the points from stats
        public class WrongWinnerException : MatchRuntimeException
        {
            private TTeam.ITeam supposedWinner;
            public WrongWinnerException(TMatch.Match match, TTeam.ITeam winner) : base(match)
            {
                supposedWinner = winner;
            }
            public override string Message
            {
                get
                {
                    return "Team " + supposedWinner.Name + " was set as winner despite losing 2 or more sets";
                }
            }
        }
    }
}
