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
            [JsonProperty]
            private List<Round> rounds = new List<Round>(2);
            private List<TPerson.Referee> referees;
            public List<Round> Rounds
            { get { return rounds; } }
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
            public TTeam.ITeam GetWinner()
            {
                if (rounds.Count == 2)
                    if (rounds[1].IsFinished())
                        return rounds[1].ListMatches[0].Winner;
                return null;
            }
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
            public void SetResult(string stat, TTeam.ITeam winner)
            {
                if (!rounds[0].IsFinished())
                    rounds[0].SetResult(stat, winner);
                else
                {
                    if (GetWinner() == null)
                        GenerateRound(new List<TTeam.ITeam> { rounds[0].ListMatches[0].Winner, rounds[0].ListMatches[1].Winner }, "final");
                    if (!rounds[1].IsFinished())
                        rounds[1].SetResult(stat, winner);
                    else
                        throw new AlreadyFinishedException(GetWinner());
                }
            }
        }
    }
}
