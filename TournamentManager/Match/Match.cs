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
			//ref is a reserved word, so I changed it to ref_ 
			public Match(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> ref_)
			{
				teamA = a;
				teamB = b;
				refA = ref_.ElementAt(0);
			}
			//Function takes a list of referees because VolleyballMatch needs 3 of them
			public virtual void setReferees(List<TPerson.Referee> ref_) { refA = ref_.ElementAt(0); }
			public void setWinner(TTeam.ITeam winner_) { winner = winner_; }
			public string getWinner() { return winner.ToString(); }
			//those virtual methods will be defined in subclasses
			public virtual void setStat(string stat) { }
			public virtual string getStat() { return null; }
			//It's just a basic try, can be changed if needed
		}

		public class TugOfWarMatch : Match
		{
			private float matchLength;
			//constructor uses a constructor of its superclass
			public TugOfWarMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> ref_) : base(a, b, ref_) { }
			//This is based on the assumption that stat is going to be in seconds (possibly with miliseconds)
			public override void setStat(string stat)
			{
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
			public DodgeballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> ref_) : base(a, b, ref_) { }
			public override void setStat(string stat)
			{
				//if stat is not a number parse will throw format exception
				try
				{
					winnerPlayersLeft = int.Parse(stat);
					if (winnerPlayersLeft <= 0)
					{
						throw new NegativePlayersNumberException();
					}
					if (winnerPlayersLeft > 6)
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
			private int[] scoreTeamA;
			private int[] scoreTeamB;
			public VolleyballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> ref_) : base(a, b, ref_)
			{
				setReferees(ref_);
			}
			public override void setReferees(List<TPerson.Referee> ref_)
			{
				base.setReferees(ref_);
				assistantReferees.AddRange( ref_.GetRange(1, 2));
			}
			//the expected format is "a: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played). b: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played)"
			public override void setStat(string stat)
			{
				stat.Split(new char[] {':', ',' });
			}
		}
	}
}
