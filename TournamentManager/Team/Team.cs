using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TTeam
    {
        public abstract class Team<T> : ITeam<T>
        {
            public string Name { get; set; }
            protected List<TPerson.Player> listPlayers = new List<TPerson.Player>();
            public int MatchesPlayed { get; set; }
            public int MatchesWon { get; set; }

            public Team(string name)
            {
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

            public abstract void SetMatchResult(bool result, string stat);

            public abstract string GetStats();

            public override string ToString()
            {
                return "Name: " + Name + ", Played matches: " + MatchesPlayed + ", Won matches: " + MatchesWon + ", List of players: " + listPlayers.ToString();
            }
        }
    }
}
