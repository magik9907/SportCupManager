using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace SportCupManagerConsole
{
    class DodgeballTeam : Team
    {
        public int PlayersEliminated { get; set; }
        public int SumOfPlayersLeft { get; set; }

        public DodgeballTeam(string name) : base(name)
        {
            this.Name = name;
        }

        public static bool operator< (DodgeballTeam a, DodgeballTeam b)
        {
            return (a.MatchesWon < b.MatchesWon) ? true : false;
        }

        public static bool operator> (DodgeballTeam a, DodgeballTeam b)
        {
            return (a.MatchesWon > b.MatchesWon) ? true : false;
        }
    }
}
