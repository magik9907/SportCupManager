using System;
using System.Collections.Generic;
using System.Linq;


namespace TournamentManager
{
	//exceptions used by class are defined below the class
	public abstract class Match
	{
		private ITeam teamA;
		private ITeam teamB;
		private ITeam winner;
		private Referee refA;
		//ref is a reserved word, so I changed it to ref_ 
		public Match(ITeam a, ITeam b, Referee ref_)
		{
			teamA = a;
			teamB = b;
			refA = ref_;
		}
		//Function takes a list of referees because VolleyballMatch needs 3 of them
		private virtual void setReferees(List<Referee> ref_) { refA = ref_.ElementAt(0); }
		public void setWinner(ITeam winner_) { winner = winner_; }
		private string getWinner() { return winner; }
		//those virtual methods will be defined in subclasses
		public virtual void setStat(string stat) { }
		public virtual string getStat() { }
	}

	class TugOfWarMatch : Match
	{
		private float matchLength;
		//constructor uses a constructor of its superclass
		public TugOfWarMatch(Team a, Team b, Referee ref_) : base(a, b, ref_) { }
		//This is based on the assumption that stat is going to be in seconds (possibly with miliseconds)
		public override void setStat(string stat)
		{
			//a safety check just in case stat is not a number 
			try
			{
				matchLength = float.Parse(stat, Globalization.CultureInfo.InvariantCulture);
			}
			//float.parse throws FormatException if stat can't be converted
			catch (FormatException e)
			{
				throw NotNumberMatchLengthException;
			}
		}
		//getStat returns time in seconds (with miliseconds)
		public override string getStat()
		{
			return matchLength.ToString();
		}
	}

	class NotNumberMatchLengthException : Exception
	{
		public override string Message
		{
			get
			{
				return "Match length is supposed to be a number!";
			}
		}
	}

	class DodgeballMatch : Match
	{
		//we might need to change that name
		private int winnerPlayersLeft;
		public DodgeballMatch(Team a, Team b, Referee ref_) : base(a, b, ref_) { }
		public override void setStat(string stat)
		{
			try
			{
				winnerPlayersLeft = int.Pasre(stat);
			}
			catch (FormatException q)
			{
				throw NotIntPlayersException;
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
}