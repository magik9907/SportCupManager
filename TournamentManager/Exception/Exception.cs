using System;
using System.Collections.Generic;
using System.Text;

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

        public class AlreadyFinishedException : Exception
        {
            private TTeam.ITeam winner;
            public AlreadyFinishedException(TTeam.ITeam winner)
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
        public class NotEnoughRefereesException : Exception
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

        public abstract class MatchCreationException : Exception
        { }

        //Exception if user wanted to set result of a match that was already played
        public class MatchAlreadyPlayedException : MatchCreationException
        {
            TTeam.ITeam winner;
            public MatchAlreadyPlayedException(TTeam.ITeam winner)
            {
                this.winner = winner;
            }
            public override string Message
            {
                get { return "The match was already played. Team " + winner.Name + " has won the match"; }
            }
        }

        //Excpetion if teamA and teamB are the same team
        public class IncorrectOpponentException : MatchCreationException
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
        public class WinnerIsNotPlayingException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Team you want to set as a winner is not playing!";
                }
            }
        }

        //Exception if match length is not a number
        public class NotNumberMatchLengthException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Match length is supposed to be a number!";
                }
            }
        }

        //Exception if match length is negative
        public class NegativeMatchLengthException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Match length is supposed to not be negative!";
                }
            }
        }

        //exception if number of players was not a whole number
        public class NotIntPlayersException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Number of players should be a whole number!";
                }
            }
        }

        //exception if number of players was negative
        public class NegativePlayersNumberException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Number of players should be positive!";
                }
            }
        }

        //Exception if number of players left was higher than players allowed on the field
        public class TooHighPlayersLeftException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Team can't have more players left than it started the game with (6)";
                }
            }
        }

        //Excpetion if name of a team passed to SetResult is not a name of any of the playing teams
        public class WrongNameInStatException : Exception
        {
            private string name;
            public WrongNameInStatException(string name)
            {
                this.name = name;
            }
            public override string Message
            {
                get { return name + " is not one of the teams playing in the match"; }
            }
        }

        //Exception if third set was played in spite of a team winning in two sets
        public class ThirdSetException : Exception
        {
            private TTeam.ITeam winner;
            public ThirdSetException(TTeam.ITeam winner)
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
        public class NoSetWinnerException : Exception
        {
            private int set;
            private int scoreRequired;
            public NoSetWinnerException(int set)
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
        public class NonIntScoreException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Score should be a whole number";
                }
            }
        }

        //Exception if score was negative
        public class NegativeScoreException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Score should be a positive number";
                }
            }
        }

        //Exception if a score entered was over the maximum points in a set
        public class TooHighScoreException : Exception
        {
            public override string Message
            {
                get
                {
                    return "No team can have more than 21 points in the first two sets and 15 in the last";
                }
            }
        }

        //Exception if string stat got separated into a different amount of strings than expected
        public class WrongStatFormatException : Exception
        {
            public override string Message
            {
                get
                {
                    return "Format of string \"stat\" is incorrect!";
                }
            }
        }

        //Exception if the team that was set as winner lost based on the points from stats
        public class WrongWinnerException : Exception
        {
            private TTeam.ITeam supposedWinner;
            public WrongWinnerException(TTeam.ITeam winner)
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
