using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
     namespace TTeam {
        abstract class Team
        {
            protected string name;
            protected List<Player> listPlayers = new List<Player>();
            protected int matchesPlayed;
            protected int matchesWon;

            public Team(string name)
            {
                this.name = name;
            }

            public void addPlayer(Player p)
            {
                listPlayers.Add(p);
            }

            public void removePlayer(Player p)
            {
                listPlayers.Remove(p);
            }

            public string ToString()
            {
                return "Name: " + name + ", Played matches: " + matchesPlayed + ", Won matches: " + matchesWon + ", List of players: " + listPlayers.ToString();
            }
        }
    }
}