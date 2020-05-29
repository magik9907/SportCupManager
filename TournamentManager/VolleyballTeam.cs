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
            if (a.Points != b.Points)
                return a.Points < b.Points;
            if (a.MatchesWon != b.MatchesWon)
                return a.MatchesWon < b.MatchesWon;
            if (a.ScoreDiff != b.ScoreDiff)
                return a.ScoreDiff < b.ScoreDiff;
            return String.Compare(a.Name, b.Name) < 0;
        }

        public static bool operator >(VolleyballTeam a, VolleyballTeam b)
        {
            if (a.Points != b.Points)
                return a.Points > b.Points;
            if (a.MatchesWon != b.MatchesWon)
                return a.MatchesWon > b.MatchesWon;
            if (a.ScoreDiff != b.ScoreDiff)
                return a.ScoreDiff > b.ScoreDiff;
            return String.Compare(a.Name, b.Name) > 0;
        }
    }
}
