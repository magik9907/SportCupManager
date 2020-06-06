﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TournamentManager.TTeam;

namespace TournamentManager
{	
	namespace TMatch
	{
		//exceptions used by class are defined below the class (will be changed in the future)
		public abstract class Match
		{
			private TTeam.ITeam teamA;
			private TTeam.ITeam teamB;
		
			private TTeam.ITeam Winner = null;
            private TPerson.Referee RefA;
			public Match(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r)
			{
				if (a == b)
					throw new IncorrectOpponentException();
				teamA = a;
				teamB = b;
				RefA = r.ElementAt(0);
			}
			public TTeam.ITeam TeamA
			{
				get { return teamA; }
			}
			public TTeam.ITeam TeamB
			{
				get { return teamB; }
			}
			//Function takes a list of referees because VolleyballMatch needs 3 of them
			public virtual void SetReferees(List<TPerson.Referee> r) { RefA = r.ElementAt(0); }
			public string GetWinner() { return Winner.ToString(); }
			//those virtual methods will be defined in subclasses
			public virtual void SetResult(string stat, TTeam.ITeam winner)
			{
				if (winner == TeamA || winner == TeamB)
					this.Winner = winner;
				else
					throw new TeamIsNotPlayingException();
			}
			public virtual string GetStat() { return null; }
			//It's just a basic try, can be changed if needed
			public Boolean isPlaying(TTeam.ITeam team)
            {
				return team == TeamA || team == TeamB;
            }

			public Boolean wasPlayed()
            {
				return Winner != null;
            }
		}

		//Excpetion if teamA and teamB are the same team
		public class IncorrectOpponentException : Exception
        {
			public override string Message
            {
                get
                {
					return "A team cannot play against itself!";
                }
            }
        }

		//Exception if team set as Winner is not playing in the match
		public class TeamIsNotPlayingException : Exception
        {
            public override string Message
            {
                get 
				{
					return "Team you want to set as a Winner is not playing!";
				}
            }
        }

		public class TugOfWarMatch : Match
		{
			private float matchLength = 0;
			//constructor uses a constructor of its superclass
			public TugOfWarMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r) { }
			//This is based on the assumption that stat is going to be in seconds (possibly with miliseconds)
			public override void SetResult(string stat, TTeam.ITeam winner)
			{
				base.SetResult(stat, winner);
				//a safety check just in case stat is not a number 
				try
				{
					matchLength = float.Parse(stat, System.Globalization.CultureInfo.InvariantCulture);
					if (matchLength < 0)
						throw new NegativeMatchLengthException();
				}
				//float.parse throws FormatException if stat can't be converted
				catch (FormatException e)
				{
					throw new NotNumberMatchLengthException();
				}
			}
			//getStat returns time in seconds (with miliseconds)
			public override string GetStat()
			{
				return matchLength.ToString();
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

		public class DodgeballMatch : Match
		{
			//we might need to change that name
			private int winnerPlayersLeft = 0;
			public DodgeballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r) { }
			public override void SetResult(string stat, TTeam.ITeam winner)
			{
				base.SetResult(stat, winner);
				//if stat is not a number parse will throw format exception
				try
				{
					winnerPlayersLeft = int.Parse(stat);
					if(winnerPlayersLeft <= 0)
					{
						throw new NegativePlayersNumberException();
					}
					if(winnerPlayersLeft > 6)
					{
						throw new TooHighPlayersLeftException();
					}
				}
				catch (FormatException q)
				{
					throw new NotIntPlayersException();
				}
			}
			public override string GetStat()
			{
				return winnerPlayersLeft.ToString();
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

		public class VolleyballMatch : Match
		{
			private List<TPerson.Referee> assistantReferees = new List<TPerson.Referee>(2);
			//the score means points gained by team in each set
			//score from x set is kept under x-1 index in the table
			private int[] scoreTeamA = new int[3] {0, 0, 0};
			private int[] scoreTeamB = new int[3] {0, 0, 0};
			public VolleyballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r)
			{
				SetReferees(r);
			}
			public override void SetReferees(List<TPerson.Referee> @ref)
			{
				base.SetReferees(@ref);
				assistantReferees.AddRange( @ref.GetRange(1, 2));
			}
			//the expected format is "a: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played). b:scoreInSet1, scoreInSet2, scoreInSet3(0 if not played)"
			public override void SetResult(string stat, TTeam.ITeam winner)
			{
                int resultCheck = 0;
				base.SetResult(stat, winner);
				//split the strings into strings containing name of the teams and their scores
				string[] tmp = stat.Split(new string[] {". ", ", ", ": "}, StringSplitOptions.RemoveEmptyEntries);
				//string should split into 8 smaller string (2 for names of teams, 6 in total for scores in sets)
				if (tmp.Length != 8)
					throw new WrongStatFormatException();
				for(int i = 0; i < 3; i++)
                {
					int scoreRequired;
					if (i != 2)
						scoreRequired = 21;
					else
						scoreRequired = 15;
					try
                    {
						scoreTeamA[i] = int.Parse(tmp[i + 1]);
						scoreTeamB[i] = int.Parse(tmp[i + 5]);
						//score should be equal or higher than 0, but equal or lower than 21 in first two sets
						//and equal or lower than 15 in the third set
						if (scoreTeamA[i] < 0 || scoreTeamB[i] < 0)
							throw new NegativeScoreException();
						if (scoreTeamA[i] > scoreRequired || scoreTeamB[i] > scoreRequired)
							throw new TooHighScoreException();
						
					}
					catch(FormatException f)
                    {
						throw new NonIntScoreException();
					}
					//Checking whether the score makes sense and reflects the Winner
					//if a team has won in 2 sets third one should end 0:0
					if (Math.Abs(resultCheck) == 2)
						if (scoreTeamA[i] != 0 || scoreTeamB[i] != 0)
							if (resultCheck > 0)
								throw new ThirdSetException(TeamA);
							else
								throw new ThirdSetException(TeamB);
					//Checking if exactly one team has reached the required points
					if (scoreTeamA[i] == scoreTeamB[i] || (scoreTeamA[i] < scoreRequired && scoreTeamB[i] < scoreRequired))
						throw new NoSetWinnerException(i+1);
					if (scoreTeamA[i] == scoreRequired)
						resultCheck++;
					if (scoreTeamB[i] == scoreRequired)
						resultCheck--;
                }
				if ((resultCheck > 0 && TeamA != winner) || (resultCheck < 0 && TeamB != winner))
					throw new WrongWinnerException(winner);
			}
			public override string GetStat()
            {
				string stat = "a: ";
				for (int i = 0; i < 3; i++)
                {
					stat += scoreTeamA[i].ToString();
					if (i != 2)
						stat += ", ";
					else
						stat += ". ";
				}
				stat += "b: ";
				for (int i = 0; i < 3; i++)
				{
					stat += scoreTeamB[i].ToString();
					if (i != 2)
						stat += ", ";
				}
				return stat;
            }
		}

		//Exception if third set was played in spite of a team winning in two sets
		public class ThirdSetException : Exception
		{
			private ITeam winner;
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
					return "No team has won the " + set + "set since no team has reached" + scoreRequired + "points while also having the lead";
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

		//Exception if the team that was set as Winner lost based on the points from stats
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
					return "Team" + supposedWinner + "was set as Winner despite losing 2 or more sets";
				}
			}
		}
	}
}
