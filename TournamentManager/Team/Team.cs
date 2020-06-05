using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
     namespace TTeam {
        public abstract class Team : ITeam
        {
            protected string name;
            protected List<TPerson.Player> listPlayers = new List<TPerson.Player>();
            protected int matchesPlayed;
            protected int matchesWon;

            public Team(string name)
            {
                this.name = name;
            }

            public void addPlayer(TPerson.Player p)
            {
                listPlayers.Add(p);
            }

            public void removePlayer(TPerson.Player p)
            {
                listPlayers.Remove(p);
            }
            public string Name
            {
                get { return name; }
            }
            public override string ToString()
            {
                return "Name: " + name + ", Played matches: " + matchesPlayed + ", Won matches: " + matchesWon + ", List of players: " + listPlayers.ToString();
            }
        }
    }
}