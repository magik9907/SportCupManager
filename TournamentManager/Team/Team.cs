using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{
    namespace TTeam
    {
        public abstract class Team : ITeam, ITeamId
        {
            private int idTeam;

            public int Id {
                get { return idTeam; }
            }

            private bool didWithdraw = false;
            public string Name { get; set; }
            [JsonProperty]
            public List<TPerson.Player> listPlayers = new List<TPerson.Player>();
            public int MatchesPlayed { get; set; }
            public int MatchesWon { get; set; }
            public bool DidWithdraw { get { return didWithdraw; } set { didWithdraw = value; } }

            public Team(string name, int id)
            {
                idTeam = id;
                if (string.IsNullOrEmpty(name))
                    throw new TException.TeamMissingNameException();

                this.Name = name;
                this.MatchesPlayed = 0;
                this.MatchesWon = 0;
            }

            public void AddPlayer(TPerson.Player p)
            {
                listPlayers.Add(p);
            }

            public void RemovePlayer(TPerson.Player p)
            {
                listPlayers.Remove(p);
            }

            public abstract void SetMatchResult(bool result, bool wasPlayedBefore, bool wasWinner, string stat);

            public virtual void Withdraw()
            {
                didWithdraw = true;
                MatchesWon = 0;
            }

            public virtual string GetStats()
            {
                return null;
            }

            public override string ToString()
            {
                return "Name: " + Name + ", Played matches: " + MatchesPlayed + ", Won matches: " + MatchesWon + ", List of players: " + listPlayers.ToString();
            }
        }
    }
}
