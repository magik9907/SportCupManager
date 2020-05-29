using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    public abstract class Team : ITeam
    {
        public string Name { get; set; }
        protected List<Player> listPlayers = new List<Player>();
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }

        public Team(string name)
        {
            this.Name = name;
        }

        public void addPlayer(Player p)
        {
            listPlayers.Add(p);
        }

        public void removePlayer(Player p)
        {
            listPlayers.Remove(p);
        }

        public override string ToString()
        {
            return "Name: " + Name + ", Played matches: " + MatchesPlayed + ", Won matches: " + MatchesWon + ", List of players: " + listPlayers.ToString();
        }
    }
}
