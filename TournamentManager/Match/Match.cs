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
			private bool isWalkover = false;
			
			[JsonConverter(typeof(TeamIdConverter))]
			public TTeam.ITeam TeamA { get { return teamA; } set { teamA = value; } }

			[JsonConverter(typeof(TeamIdConverter))]
			public TTeam.ITeam TeamB { get { return teamB; } set { teamB = value; } }

			[JsonConverter(typeof(TeamIdConverter))]
			public TTeam.ITeam Winner { get { return winner; } set { winner = value; } }
			
			[JsonProperty]
			[JsonConverter(typeof(RefereeIdConverter))]
			private TPerson.Referee RefA;
			
			public TPerson.Referee Referee { set
                {
					RefA = value;
                } 
			}
			
			public bool IsWalkover { get { return isWalkover; } set { isWalkover = value; } }
			
			public Match(Match match)
            {
				this.teamA = match.teamA;
				this.teamB = match.teamB;
				this.winner = match.winner;
				this.RefA = match.RefA;
            }
			
			public virtual Match CreateCopy()
			{
				return null;
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
			
			public virtual List<TPerson.Referee> GetReferees()
            {
				return new List<TPerson.Referee>() { RefA };
            }
			
			//those virtual methods will be defined in subclasses
			public virtual void SetResult(string stat, TTeam.ITeam winner)
			{
				if (winner == TeamA || winner == TeamB || winner == null)
					this.winner = winner;
				else
					throw new WinnerIsNotPlayingException(CreateCopy());
			}
			
			public virtual string GetStat() { return null; }
			//It's just a basic try, can be changed if needed
			
			public bool IsPlaying(TTeam.ITeam team)
            {
				return team == TeamA || team == TeamB;
            }

			public bool WasPlayed()
            {
				return Winner != null || IsWalkover;
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
			
			public virtual void Walkover(TTeam.ITeam absentee)
            {
				if (IsWalkover == true)
					winner = null;
				isWalkover = true;
			}
		}

		public class TugOfWarMatch : Match
		{
			[JsonProperty]
			private float matchLength = 0;
			
			//constructor uses a constructor of its superclass
			public TugOfWarMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r) { }
			
			public TugOfWarMatch(TugOfWarMatch match) : base(match)
			{
				this.matchLength = match.matchLength;
			}
			
			public override Match CreateCopy()
			{
				return new TugOfWarMatch(this);
			}
			
			public void SetResult(float matchLength)
			{
				this.matchLength = matchLength;
				if(Winner != null)
                {
					TeamA.SetMatchResult(TeamA == Winner, false, false, matchLength + " - " + 0.ToString());
					TeamB.SetMatchResult(TeamB == Winner, false, false, matchLength + " - " + 0.ToString());
				}
			}

			//This is based on the assumption that stat is going to be in seconds (possibly with miliseconds)
			public override void SetResult(string stat, TTeam.ITeam winner)
			{
				float tmp = matchLength;
				//a safety check just in case stat is not a number 
				try
				{
					matchLength = float.Parse(stat);
					if (matchLength < 0)
                    {
						matchLength = 0;
						throw new NegativeMatchLengthException(CreateCopy());
					}
				}
				//float.parse throws FormatException if stat can't be converted
				catch (FormatException)
				{
					throw new NotNumberMatchLengthException(CreateCopy());
				}
				winner.SetMatchResult(true, tmp != 0, tmp != 0 && this.Winner == winner, stat + " - " + tmp.ToString());
				if (winner == TeamA)
					TeamB.SetMatchResult(false, tmp != 0, tmp != 0 && this.Winner == TeamB, stat + " - " + tmp.ToString());
				else
					TeamA.SetMatchResult(false, tmp != 0, tmp != 0 && this.Winner == TeamA, stat + " - " + tmp.ToString());
				base.SetResult(stat, winner);
			}
			
			//getStat returns time in seconds (with miliseconds)
			public override string GetStat()
			{
				return matchLength.ToString();
			}
            
			public override void Walkover(ITeam absentee)
            {
				if (IsWalkover == false)
				{
					if (absentee == TeamA)
						SetResult("0", TeamB);
					else
						SetResult("0", TeamA);
				}
				else
					matchLength = 0;
				base.Walkover(absentee);
			}
        }

		public class DodgeballMatch : Match
		{
			//we might need to change that name
			[JsonProperty]
			private int winnerPlayersLeft = 0;
			
			public DodgeballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r) { }
			
			public DodgeballMatch(DodgeballMatch match) : base(match)
			{
				this.winnerPlayersLeft = match.winnerPlayersLeft;
			}
			
			public override Match CreateCopy()
			{
				return new DodgeballMatch(this);
			}

			public void SetResult(int wPlayersLeft)
			{
				winnerPlayersLeft = wPlayersLeft;
				if(Winner == TeamA)
                {
					TeamA.SetMatchResult(TeamA == Winner, false, false, winnerPlayersLeft + ", " + 6);
					TeamB.SetMatchResult(TeamB == Winner, false, false, 0 + ", " + (6-winnerPlayersLeft));
				}
				if (Winner == TeamB)
				{
					TeamB.SetMatchResult(TeamB == Winner, false, false, winnerPlayersLeft + ", " + 6);
					TeamA.SetMatchResult(TeamA == Winner, false, false, 0 + ", " + (6 - winnerPlayersLeft));
				}
			}

			public override void SetResult(string stat, TTeam.ITeam winner)
			{
				int playersEliminatedChange = 0, playersLeftChange = 0;
				if (this.Winner == winner)
					playersLeftChange = winnerPlayersLeft;
				else
                {
					if(this.Winner != null)
                    {
						playersEliminatedChange = 6 - winnerPlayersLeft;
                    }
                }
				//if stat is not a number parse will throw format exception
				try
				{
					winnerPlayersLeft = int.Parse(stat);
					if(winnerPlayersLeft <= 0)
					{
						winnerPlayersLeft = 0;
						throw new NegativePlayersNumberException(CreateCopy());
					}
					if(winnerPlayersLeft > 6)
					{
						winnerPlayersLeft = 0;
						throw new TooHighPlayersLeftException(CreateCopy());
					}
				}
				catch (FormatException)
				{
					throw new NotIntPlayersException(CreateCopy());
				}
				winner.SetMatchResult(true, playersEliminatedChange != 0 || playersLeftChange != 0, (playersEliminatedChange != 0 || playersLeftChange != 0) && this.Winner == winner, (winnerPlayersLeft - playersLeftChange).ToString() + ", " + playersEliminatedChange.ToString());
				if (winner == TeamA)
					TeamB.SetMatchResult(false, playersEliminatedChange != 0 || playersLeftChange != 0, (playersEliminatedChange != 0 || playersLeftChange != 0) && this.Winner == TeamB, (-playersLeftChange).ToString() + ", " + (6 - winnerPlayersLeft - playersEliminatedChange).ToString());
				else
					TeamA.SetMatchResult(false, playersEliminatedChange != 0 || playersLeftChange != 0, (playersEliminatedChange != 0 || playersLeftChange != 0) && this.Winner == TeamA, (-playersLeftChange).ToString() + ", " + (6 - winnerPlayersLeft - playersEliminatedChange).ToString());
				base.SetResult(stat, winner);
			}
			
			public override string GetStat()
			{
				return winnerPlayersLeft.ToString();
			}
			
			public override void Walkover(ITeam absentee)
			{
				if (IsWalkover == false)
				{
					if (absentee == TeamA)
						SetResult("6", TeamB);
					else
						SetResult("6", TeamA);
				}
				else
					winnerPlayersLeft = 0;
				base.Walkover(absentee);
			}
		}

		public class VolleyballMatch : Match
		{
			[JsonProperty]
			[JsonConverter(typeof(RefereeIdConverter))]
			private List<TPerson.Referee> assistantReferees = new List<TPerson.Referee>(2);
			//the score means points gained by teams in each set
			[JsonProperty]
			private int[] scoreTeamA = new int[3] {0, 0, 0};
			[JsonProperty]
			private int[] scoreTeamB = new int[3] {0, 0, 0};
			
			//Main constructor
			public VolleyballMatch(TTeam.ITeam a, TTeam.ITeam b, List<TPerson.Referee> r) : base(a, b, r)
			{
				SetReferees(r);
			}
			
			//copying constructor
			public VolleyballMatch(VolleyballMatch match) : base(match)
			{
				this.scoreTeamA = match.scoreTeamA;
				this.scoreTeamB = match.scoreTeamB;
			}


			private List<TPerson.Referee> AssistantReferees
			{
				set { assistantReferees = value; }
			}
			//Copying method
			public override Match CreateCopy()
			{
				return new VolleyballMatch(this);
			}
			
			public override void SetReferees(List<TPerson.Referee> r)
			{
				
				assistantReferees.AddRange( r.GetRange(1, 2));
			}

			//returns a list of referees
			public override List<TPerson.Referee> GetReferees()
			{
				List<TPerson.Referee> tmp = base.GetReferees();
				tmp.AddRange(assistantReferees);
				return tmp;
			}

			//this method is required for saving results in files
			public void SetResult(int[] scoreA, int[] scoreB)
			{
				scoreTeamA = scoreA;
				scoreTeamB = scoreB;
				int resultCheck = 0, scoreDiff = 0;
				for(int i = 0; i < 3; i++)
                {
					if (scoreA[i] > scoreB[i])
						resultCheck++;
					else if (scoreTeamA[i] < scoreTeamB[i])
						resultCheck--;
					scoreDiff = scoreTeamA[i] - scoreTeamB[i];
                }
				if (Winner == TeamA)
				{
					TeamA.SetMatchResult(true, false, false, (1 + Math.Abs(resultCheck)).ToString() + ", " + (scoreDiff).ToString());
					TeamB.SetMatchResult(false, false, false, (2 - Math.Abs(resultCheck)).ToString() + ", " + (- scoreDiff).ToString());
				}
				if(Winner == TeamB)
				{
					TeamA.SetMatchResult(false, false, false, (2 - Math.Abs(resultCheck)).ToString() + ", " + (- scoreDiff).ToString());
					TeamB.SetMatchResult(true, false, false, (1 + Math.Abs(resultCheck)).ToString() + ", " + (scoreDiff).ToString());
				}
			}

			//the expected format is "team1.Name: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played). team2.Name: scoreInSet1, scoreInSet2, scoreInSet3(0 if not played)"
			public override void SetResult(string stat, TTeam.ITeam winner)
			{
				int resultCheck = 0, scoreDiff = 0;
				int earlierScoreDiff = 0, earlierPoints = 0;
				if (WasPlayed())
				{
					for (int i = 0; i < 3; i++)
					{
						earlierScoreDiff += scoreTeamA[i] - scoreTeamB[i];
						if (scoreTeamA[i] > scoreTeamB[i])
							earlierPoints++;
						if (scoreTeamA[i] < scoreTeamB[i])
							earlierPoints--;
					}
					if (earlierPoints > 0)
						earlierPoints++;
					else
						earlierPoints += 2;
				}
				//split the strings into strings containing name of the teams and their scores
				string[] tmp = stat.Split(new string[] {". ", ", ", ": "}, StringSplitOptions.RemoveEmptyEntries);
				//string should split into 8 smaller string (2 for names of teams, 6 in total for scores in sets)
				if (tmp.Length != 8)
					throw new WrongStatFormatException(CreateCopy());
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
									throw new WrongNameInStatException(CreateCopy(), tmp[4]);
								}
								else
                                {
									ToString();
									throw new WrongNameInStatException(CreateCopy(), tmp[0]);
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
							throw new NegativeScoreException(CreateCopy());
						}
						if (scoreTeamA[i] > scoreRequired || scoreTeamB[i] > scoreRequired)
						{
							for (int j = 0; j < 3; j++)
							{
								scoreTeamA[j] = 0;
								scoreTeamB[j] = 0;
							}
							throw new TooHighScoreException(CreateCopy());
						}
					}
					catch (FormatException)
					{
						throw new NonIntScoreException(CreateCopy());
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
								throw new ThirdSetException(CreateCopy(), TeamA);
							}
							else
							{
								for (int j = 0; j < 3; j++)
								{
									scoreTeamA[j] = 0;
									scoreTeamB[j] = 0;
								}
								throw new ThirdSetException(CreateCopy(), TeamB);
							}
					}
					//Checking if exactly one team has reached the required points
					else
                    {
						if (scoreTeamA[i] == scoreTeamB[i] || (scoreTeamA[i] < scoreRequired && scoreTeamB[i] < scoreRequired))
							throw new NoSetWinnerException(CreateCopy(), i + 1);
						if (scoreTeamA[i] == scoreRequired)
							resultCheck++;
						if (scoreTeamB[i] == scoreRequired)
							resultCheck--;
					scoreDiff += scoreTeamA[i] - scoreTeamB[i];
					}
                }
				//checking if a team which should have won by what the stat indicates was set as a winner
				if ((resultCheck > 0 && TeamA != winner) || (resultCheck < 0 && TeamB != winner))
				{
					for (int j = 0; j < 3; j++)
					{
						scoreTeamA[j] = 0;
						scoreTeamB[j] = 0;
					}
					throw new WrongWinnerException(CreateCopy(), winner);
				}
				int temp = 0;
				if(WasPlayed())
					temp = 3 - earlierPoints;
				if (winner == TeamA)
                {
					TeamA.SetMatchResult(true, earlierScoreDiff != 0, earlierScoreDiff != 0 && this.Winner == TeamA, (1 + Math.Abs(resultCheck) - earlierPoints).ToString() + ", " + (scoreDiff-earlierScoreDiff).ToString());
					TeamB.SetMatchResult(false, earlierScoreDiff != 0, earlierScoreDiff != 0 && this.Winner == TeamB, (2 - Math.Abs(resultCheck) - temp).ToString() + ", " + (earlierScoreDiff-scoreDiff).ToString());
				}
				else
                {
					TeamA.SetMatchResult(false, earlierScoreDiff != 0, earlierScoreDiff != 0 && this.Winner == TeamA, (2 - Math.Abs(resultCheck) - temp).ToString() + ", " + (earlierScoreDiff - scoreDiff).ToString());
					TeamB.SetMatchResult(true, earlierScoreDiff != 0, earlierScoreDiff != 0 && this.Winner == TeamB, (1 + Math.Abs(resultCheck) - earlierPoints).ToString() + ", " + (scoreDiff - earlierScoreDiff).ToString());
				}
				base.SetResult(stat, winner);
			}

			//returns scores for each team set by set
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

			//sets the match as a Walkover
            public override void Walkover(ITeam absentee)
            {
				if(IsWalkover == false)
                {
					if (TeamA == absentee)
						SetResult(TeamB.Name + ": 21, 21, 0. " + absentee.Name + ": 0, 0, 0", TeamB);
					if (TeamB == absentee)
						SetResult(TeamA.Name + ": 21, 21, 0. " + absentee.Name + ": 0, 0, 0", TeamA);
				}
				else
					scoreTeamB = scoreTeamA = new int[3]{ 0, 0, 0};
				base.Walkover(absentee);
			}
        }
	}
}
