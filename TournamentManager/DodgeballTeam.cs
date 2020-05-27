using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace SportCupManagerConsole
{
    class DodgeballTeam : Team
    {
        private int playersEliminated;
        private int sumOfPlayersLeft;

        public DodgeballTeam(string name) : base(name)
        {
            this.name = name;
        }

        public static bool operator< (DodgeballTeam a, DodgeballTeam b)
        {
            return (a.matchesWon < b.matchesWon) ? true : false;
        }

        public static bool operator> (DodgeballTeam a, DodgeballTeam b)
        {
            return (a.matchesWon > b.matchesWon) ? true : false;
        }

        public void setPlayersEliminated(int eliminated)
        {
            this.playersEliminated = eliminated;
        }

        public void setSumOfPlayersLeft(int sum)
        {
            this.sumOfPlayersLeft = sum;
        }
    }
}
