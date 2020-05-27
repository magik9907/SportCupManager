using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    class TugOfWarTeam : Team
    {
        public float AvWinTime { get; set; }
        public float AvLossTime { get; set; }

        public TugOfWarTeam(string name) : base(name)
        {
            this.Name = name;
        }

        public static bool operator <(TugOfWarTeam a, TugOfWarTeam b)
        {
            return (a.MatchesWon < b.MatchesWon) ? true : false;
        }

        public static bool operator >(TugOfWarTeam a, TugOfWarTeam b)
        {
            return (a.MatchesWon > b.MatchesWon) ? true : false;
        }
    }
}
