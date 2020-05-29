using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace TournamentManager
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
            if (a.MatchesWon != b.MatchesWon)
                return a.MatchesWon < b.MatchesWon;
            if (a.PlayersEliminated != b.PlayersEliminated)
                return a.PlayersEliminated < b.PlayersEliminated;
            if (a.SumOfPlayersLeft != b.SumOfPlayersLeft)
                return a.SumOfPlayersLeft < b.SumOfPlayersLeft;
            return String.Compare(a.Name, b.Name) < 0;
        }

        public static bool operator> (DodgeballTeam a, DodgeballTeam b)
        {
            if (a.MatchesWon != b.MatchesWon)
                return a.MatchesWon > b.MatchesWon;
            if (a.PlayersEliminated != b.PlayersEliminated)
                return a.PlayersEliminated > b.PlayersEliminated;
            if (a.SumOfPlayersLeft != b.SumOfPlayersLeft)
                return a.SumOfPlayersLeft > b.SumOfPlayersLeft;
            return String.Compare(a.Name, b.Name) > 0;
        }
    }
}
