using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TTeam
    {
        public class VolleyballTeam : Team<VolleyballTeam>
        {
            public int Points { get; set; }
            public int ScoreDiff { get; set; }

            public VolleyballTeam(string name) : base(name)
            {
                this.Name = name;
                this.Points = 0;
                this.ScoreDiff = 0;
            }

            public static bool operator<(VolleyballTeam a, VolleyballTeam b)
            {
                if (a.Points != b.Points)
                    return a.Points < b.Points;
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon < b.MatchesWon;
                if (a.ScoreDiff != b.ScoreDiff)
                    return a.ScoreDiff < b.ScoreDiff;
                return String.Compare(a.Name, b.Name) < 0;
            }

            public static bool operator>(VolleyballTeam a, VolleyballTeam b)
            {
                if (a.Points != b.Points)
                    return a.Points > b.Points;
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon > b.MatchesWon;
                if (a.ScoreDiff != b.ScoreDiff)
                    return a.ScoreDiff > b.ScoreDiff;
                return String.Compare(a.Name, b.Name) > 0;
            }

            public override void SetMatchResult(bool result, string stat)
            {
                this.MatchesPlayed++;
                if (result)
                    this.MatchesWon++;
                string[] args = stat.Split(", ");
                Points += Int32.Parse(args[0]); // adding to Points
                ScoreDiff += Int32.Parse(args[1]); // adding to ScoreDiff
            }

            public override string GetStats()
            {
                return Points + ", " + MatchesWon + ", " + ScoreDiff;
            }
        }
    }
}
