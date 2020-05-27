using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    class VolleyballTeam : Team
    {
        public int Points { get; set; }
        public int ScoreDiff { get; set; }

        public VolleyballTeam(string name) : base(name)
        {
            this.Name = name;
        }

        public static bool operator <(VolleyballTeam a, VolleyballTeam b)
        {
            return (a.MatchesWon < b.MatchesWon) ? true : false;
        }

        public static bool operator >(VolleyballTeam a, VolleyballTeam b)
        {
            return (a.MatchesWon > b.MatchesWon) ? true : false;
        }
    }
}
