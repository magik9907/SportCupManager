using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TournamentManager
{	
	namespace TMatch
	{
		//exceptions used by class are defined below the class (will be changed in the future)
		public abstract class Match
		{
			private TTeam.ITeam teamA;
			private TTeam.ITeam teamB;
			private TTeam.ITeam winner;
			private TPerson.Referee refA;
			public Match(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> @ref)
			{
				teamA = a;
				teamB = b;
				refA = @ref.ElementAt(0);
			}
			//Function takes a list of referees because VolleyballMatch needs 3 of them
			public virtual void setReferees(List<TPerson.Referee> ref_) { refA = ref_.ElementAt(0); }
			public void setWinner(TTeam.ITeam winner_) { winner = winner_; }
			public string getWinner() { return winner.ToString(); }
			//those virtual methods will be defined in subclasses
			public virtual void setResult(string stat, TTeam.ITeam @winner)
			{
				if (@winner == teamA || winner == teamB)
					winner = winner;
				else
					throw new TeamIsNotPlayingException();
			}
			public virtual string getStat() { return null; }
			//It's just a basic try, can be changed if needed
		}

		//Exception if team set as winner is not playing in the match
		public class TeamIsNotPlayingException : Exception
        {
            public override string Message
            {
                get 
				{
					return "Team you want to set as a winner is not playing!";
				}
            }
        }

		public class TugOfWarMatch : Match
		{
			private float matchLength;
			//constructor uses a constructor of its superclass
			public TugOfWarMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> @ref) : base(a, b, @ref) { }
			//This is based on the assumption that stat is going to be in seconds (possibly with miliseconds)
			public override void setResult(string stat, TTeam.ITeam @winner)
			{
				base.setResult(stat, @winner);
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
			public override string getStat()
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
			private int winnerPlayersLeft;
			public DodgeballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> @ref) : base(a, b, @ref) { }
			public override void setResult(string stat, TTeam.ITeam @winner)
			{
				base.setResult(stat, @winner);
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
			public override string getStat()
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
			private int[] scoreTeamA = new int[3];
			private int[] scoreTeamB = new int[3];
			public VolleyballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> @ref) : base(a, b, @ref)
			{
				setReferees(@ref);
			}
			public override void setReferees(List<TPerson.Referee> @ref)
			{
				base.setReferees(@ref);
				assistantReferees.AddRange( @ref.GetRange(1, 2));
			}
			//the expected format is "a: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played). b:scoreInSet1, scoreInSet2, scoreInSet3(0 if not played)"
			public override void setResult(string stat, TTeam.ITeam @winner)
			{
				int resultCheck;
				base.setResult(stat, @winner);
				//split the strings into strings containing name of the teams and their scores
				string[] tmp = stat.Split(new string[] {". ", ", ", ": "});
				//string should split into 8 smaller string (2 for names of teams, 6 in total for scores in sets)
				if (tmp.Length != 8)
					throw new WrongStatFormatException();
				for(int i = 0; i < 3; i++)
                {
					scoreTeamA[i] = int.Parse(tmp[i + 1]);
					scoreTeamB[i] = int.Parse(tmp[i + 5]);
					if (scoreTeamA[i] > scoreTeamB[i])
						resultCheck++;
					if (scoreTeamB[i] > scoreTeamA[i])
						resultCheck--;
                }
				if ((resultCheck > 0 && teamA != @winner) || (resultCheck < 0 && scoreTeamB != @winner))
					throw new WrongWinnerException(@winner);
			}
		}

		//Exception if string stat got seprated into a different amount of strings than expected
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
			public TTeam.ITeam supposedWinner;
			public WrongWinnerException(TTeam.ITeam winner)
            {
				supposedWinner = winner;
            }
			public override string Message
			{
				get
				{
					return "Team" + supposedWinner.ToString() + "was set as winner despite losing 2 or more sets";
				}
			}
		}
	}
}
