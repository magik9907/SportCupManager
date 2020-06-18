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
        public class PlayOff
        {
            private List<Round> rounds = new List<Round>(2);
            private List<TPerson.Referee> referees;
            [JsonIgnore]
           
            public List<TPerson.Referee> Referees
            {
                set { referees = value; }
            }
            
            public void addReferee(TPerson.Referee refe)
            {
                if (referees == null)
                    referees = new List<TPerson.Referee>();
                referees.Add(refe);
            }
            
            public List<Round> Rounds
            {
                get { 
                    return rounds;
                } 
                set { 
                    rounds = value;
                } 
            }
            
            //constructor for reading from files
            public PlayOff() { }

            //Main Constructor
            public PlayOff(List<TTeam.ITeam> t, List<TPerson.Referee> referees, int[] startDate)
            {
                //semis and finals will be generated on two consecutive days
                //PlayOffs require 4 teams
                if (t.Count != 4)
                    throw new NotEnoughtTeamsNumber(t.Count);
                //this part checks whether or not there are any duplicate teams on the list
                for (int i = 0; i < t.Count; i++)
                    for (int j = i + 1; j + i < t.Count; j++)
                        if (t[i] == t[j])
                            throw new DuplicateTeamException(t[i]);
                rounds.Add(new Round("semi-finals", startDate));
                try
                {
                    startDate[0]++;
                    rounds.Add(new Round("final", startDate));
                }
                catch (WrongDayException)
                {
                    startDate[0] = 1;
                    if (startDate[1] == 12)
                    {
                        startDate[1] = 1;
                        startDate[2]++;
                    }
                    else
                        startDate[1]++;
                    rounds.Add(new Round("final", startDate));
                }
                this.referees = referees;
                GenerateRound(t, "semi-finals");
            }
            
            //copying constructor
            public PlayOff(PlayOff copy)
            {
                this.referees = copy.referees;
                this.rounds = copy.rounds;
            }
            
            //copying method
            public PlayOff CreateCopy()
            {
                return new PlayOff(this);
            }
            
            //return a winner of the PlayOff
            public TTeam.ITeam GetWinner()
            {
                if (rounds.Count == 2)
                    if (rounds[1].IsFinished())
                        return rounds[1].ListMatches[0].Winner;
                return null;
            }
            
            //method generates a round by a said name
            private void GenerateRound(List<TTeam.ITeam> teams, string name)
            {
                int roundNumber, refNumber = 1;
                if (name == "final")
                    roundNumber = 1;
                else
                    roundNumber = 0;
                if (teams[0] is VolleyballTeam)
                    refNumber = 3;
                for (int i = 0; i < teams.Count / 2; i++)
                    rounds[roundNumber].AddMatch(TMatch.Match.CreateMatch(teams[i], teams[teams.Count - 1 - i], referees.GetRange(i * refNumber, refNumber)));
            }
            
            //SetResult sets a result of a match played by a team
            public void SetResult(string stat, TTeam.ITeam winner)
            {
                if (!rounds[0].IsFinished())
                    rounds[0].SetResult(stat, winner);
                else
                {
                    if (!rounds[1].IsFinished())
                        rounds[1].SetResult(stat, winner);
                    else
                        throw new AlreadyFinishedException(CreateCopy(), GetWinner());
                }
                if (rounds[0].IsFinished() && !rounds[1].IsFinished())
                    GenerateRound(new List<TTeam.ITeam> { rounds[0].ListMatches[0].Winner, rounds[0].ListMatches[1].Winner }, "final");
            }
            
            //withdraw a team from the PlayOffs
            public void WithdrawTeam(TTeam.ITeam t)
            {
                if (rounds[0].GetMatch(t).Winner == t)
                {
                    rounds[0].GetMatch(t).Walkover(t);
                    if (rounds[0].IsFinished())
                    {
                        if (rounds.Count == 2)
                            rounds.RemoveAt(1);
                        GenerateRound(new List<TTeam.ITeam> { rounds[0].ListMatches[0].Winner, rounds[0].ListMatches[1].Winner }, "final");
                    }
                }
                t.Withdraw();
            }
        }
    }
}
