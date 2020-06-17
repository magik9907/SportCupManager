using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TournamentManager.TException;
using TournamentManager.TRound;
using TournamentManager.TTeam;
using Newtonsoft.Json;

namespace TournamentManager
{	
	namespace TMatch
	{
		//exceptions used by class are defined below the class (will be changed in the future)
		public abstract class Match
		{
			private TTeam.ITeam teamA;
			private TTeam.ITeam teamB;
			private TTeam.ITeam winner = null;
			[JsonConverter(typeof(TeamIdConverter))]
			public TTeam.ITeam TeamA { get { return teamA; } set { teamA = value; } }

			[JsonConverter(typeof(TeamIdConverter))]
			public TTeam.ITeam TeamB { get { return teamB; } set { teamB = value; } }

			[JsonConverter(typeof(TeamIdConverter))]
			public TTeam.ITeam Winner { get { return winner; } set { winner = value; } }
			[JsonProperty]
			[JsonConverter(typeof(RefereeIdConverter))]
			private TPerson.Referee RefA ;

			public TPerson.Referee Referee { set
                {
					RefA = value;
                } 
			}

			public Match(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r)
			{
				if (a == b)
					throw new IncorrectOpponentException();
				teamA = a;
				teamB = b;
				RefA = r.ElementAt(0);
			}
			//Function takes a list of referees because VolleyballMatch needs 3 of them
			public virtual void SetReferees(List<TPerson.Referee> r) { RefA = r.ElementAt(0); }
			//those virtual methods will be defined in subclasses
			public virtual void SetResult(string stat, TTeam.ITeam winner)
			{
				if (Winner != null)
					throw new MatchAlreadyPlayedException(winner);
				if (winner == TeamA || winner == TeamB)
					this.winner = winner;
				else
					throw new WinnerIsNotPlayingException();
			}
			public virtual string GetStat() { return null; }
			//It's just a basic try, can be changed if needed
			public Boolean isPlaying(TTeam.ITeam team)
            {
				return team == TeamA || team == TeamB;
            }

			public Boolean WasPlayed()
            {
				return Winner != null;
            }
			public static Match CreateMatch(TTeam.ITeam team1, TTeam.ITeam team2, List<TPerson.Referee> refs)
            {
				if (team1 is DodgeballTeam)
					return new TMatch.DodgeballMatch(team1, team2, refs);
				else
					if (team1 is TugOfWarTeam)
						return new TMatch.TugOfWarMatch(team1, team2, refs);
					else
						return new TMatch.VolleyballMatch(team1, team2, refs);
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
                    {
						matchLength = 0;
						throw new NegativeMatchLengthException();
					}
				}
				//float.parse throws FormatException if stat can't be converted
				catch (FormatException)
				{
					throw new NotNumberMatchLengthException();
				}
				winner.SetMatchResult(true, stat);
				if (winner == TeamA)
					TeamB.SetMatchResult(false, stat);
				else
					TeamA.SetMatchResult(false, stat);
			}
			//getStat returns time in seconds (with miliseconds)
			public override string GetStat()
			{
				return matchLength.ToString();
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
						winnerPlayersLeft = 0;
						throw new NegativePlayersNumberException();
					}
					if(winnerPlayersLeft > 6)
					{
						winnerPlayersLeft = 0;
						throw new TooHighPlayersLeftException();
					}
				}
				catch (FormatException)
				{
					throw new NotIntPlayersException();
				}
				winner.SetMatchResult(true, stat);
				if (winner == TeamA)
					TeamB.SetMatchResult(false, (6 - winnerPlayersLeft).ToString());
				else
					TeamA.SetMatchResult(false, (6 - winnerPlayersLeft).ToString());
			}
			public override string GetStat()
			{
				return winnerPlayersLeft.ToString();
			}
		}

		public class VolleyballMatch : Match
		{
			[JsonProperty]
			[JsonConverter(typeof(RefereeIdConverter))]
			private List<TPerson.Referee> assistantReferees = new List<TPerson.Referee>(2);
			//the score means points gained by team in each set
			private int[] scoreTeamA = new int[3] {0, 0, 0};
			private int[] scoreTeamB = new int[3] {0, 0, 0};
			public VolleyballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r)
			{
				SetReferees(r);
			}
			private List<TPerson.Referee> AssistantReferees
            {
				set { assistantReferees = value; }
            }
			public override void SetReferees(List<TPerson.Referee> r)
			{
				
				assistantReferees.AddRange( r.GetRange(1, 2));
			}
            //the expected format is "team1.Name: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played). team2.Name:scoreInSet1, scoreInSet2, scoreInSet3(0 if not played)"
            public override void SetResult(string stat, TTeam.ITeam winner)
			{
				int resultCheck = 0, scoreDiff = 0;
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
						if(tmp[0].Equals(TeamA.Name) && tmp[4].Equals(TeamB.Name))
                        {
							scoreTeamA[i] = int.Parse(tmp[i + 1]);
							scoreTeamB[i] = int.Parse(tmp[i + 5]);
						}
						else
                        {
							if (tmp[0].Equals(TeamB.Name) && tmp[4].Equals(TeamA.Name))
							{
								scoreTeamB[i] = int.Parse(tmp[i + 1]);
								scoreTeamA[i] = int.Parse(tmp[i + 5]);
							}
							else
                            {
								if (TeamA.Name.Equals(tmp[0]) || TeamB.Name.Equals(tmp[0]))
                                {
									Console.WriteLine(TeamA.Name + " " + TeamB.Name);
									throw new WrongNameInStatException(tmp[4]);
								}
								else
                                {
									ToString();
									throw new WrongNameInStatException(tmp[0]);
								}
                            }

						}

						//score should be equal or higher than 0, but equal or lower than 21 in first two sets
						//and equal or lower than 15 in the third set
						if (scoreTeamA[i] < 0 || scoreTeamB[i] < 0)
						{
							for (int j = 0; j < 3; j++)
							{
								scoreTeamA[j] = 0;
								scoreTeamB[j] = 0;
							}
							throw new NegativeScoreException();
						}
						if (scoreTeamA[i] > scoreRequired || scoreTeamB[i] > scoreRequired)
						{
							for (int j = 0; j < 3; j++)
							{
								scoreTeamA[j] = 0;
								scoreTeamB[j] = 0;
							}
							throw new TooHighScoreException();
						}
					}
					catch (FormatException)
					{
						throw new NonIntScoreException();
					}
					//Checking whether the score makes sense and reflects the winner
					//if a team has won in 2 sets third one should end 0:0
					if (Math.Abs(resultCheck) == 2)
                    {
						if (scoreTeamA[i] != 0 || scoreTeamB[i] != 0)
							if (resultCheck > 0)
							{
								for (int j = 0; j < 3; j++)
								{
									scoreTeamA[j] = 0;
									scoreTeamB[j] = 0;
								}
								throw new ThirdSetException(TeamA);
							}
							else
							{
								for (int j = 0; j < 3; j++)
								{
									scoreTeamA[j] = 0;
									scoreTeamB[j] = 0;
								}
								throw new ThirdSetException(TeamB);
							}
					}
					//Checking if exactly one team has reached the required points
					else
                    {
						if (scoreTeamA[i] == scoreTeamB[i] || (scoreTeamA[i] < scoreRequired && scoreTeamB[i] < scoreRequired))
							throw new NoSetWinnerException(i + 1);
						if (scoreTeamA[i] == scoreRequired)
							resultCheck++;
						if (scoreTeamB[i] == scoreRequired)
							resultCheck--;
					scoreDiff += scoreTeamA[i] - scoreTeamB[i];
					}
                }
				if ((resultCheck > 0 && TeamA != winner) || (resultCheck < 0 && TeamB != winner))
				{
					for (int j = 0; j < 3; j++)
					{
						scoreTeamA[j] = 0;
						scoreTeamB[j] = 0;
					}
					throw new WrongWinnerException(winner);
				}
				if (winner == TeamA)
                {
					TeamA.SetMatchResult(true, (1+ Math.Abs(resultCheck)).ToString() + ", " + scoreDiff.ToString());
					TeamB.SetMatchResult(false, (2 - Math.Abs(resultCheck)).ToString() + ", " + (-scoreDiff).ToString());
				}
				else
                {
					TeamA.SetMatchResult(false, (2 - Math.Abs(resultCheck)).ToString() + ", " + scoreDiff.ToString());
					TeamB.SetMatchResult(true, (1 + Math.Abs(resultCheck)).ToString() + ", " + (-scoreDiff).ToString());
				}
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
	}
}
