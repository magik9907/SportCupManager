using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TTeam
    {
        public class VolleyballTeam : Team
        {
            public int Points { get; set; }
            public int ScoreDiff { get; set; }

            public VolleyballTeam(string name, int id) : base(name,id)
            {
                this.Points = 0;
                this.ScoreDiff = 0;
            }

            public VolleyballTeam(string name, int id, List<TPerson.Player> players) : this(name, id)
            {
                this.listPlayers.AddRange(players);
            }
            public VolleyballTeam(string name, int id, List<TPerson.Player> players, Dictionary<string, float> stats) : this(name, id, players)
            {
                Points = (int)Math.Round(stats["Points"]);
                ScoreDiff = (int)Math.Round(stats["ScoreDiff"]);
                MatchesPlayed = (int)Math.Round(stats["MatchesPlayed"]);
                MatchesWon = (int)Math.Round(stats["MatchesWon"]);
        }

            public static bool operator <(VolleyballTeam a, VolleyballTeam b)
            {
                if (a.Points != b.Points)
                    return a.Points < b.Points;
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon < b.MatchesWon;
                if (a.ScoreDiff != b.ScoreDiff)
                    return a.ScoreDiff < b.ScoreDiff;
                return String.Compare(a.Name, b.Name) > 0;
            }

            public static bool operator >(VolleyballTeam a, VolleyballTeam b)
            {
                if (a.Points != b.Points)
                    return a.Points > b.Points;
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon > b.MatchesWon;
                if (a.ScoreDiff != b.ScoreDiff)
                    return a.ScoreDiff > b.ScoreDiff;
                return String.Compare(a.Name, b.Name) < 0;
            }

            public override void SetMatchResult(bool result, bool wasPlayedBefore, bool wasWinner, string stat)
            {
                if(!wasPlayedBefore)
                    this.MatchesPlayed++;
                if (result && !wasWinner)
                    this.MatchesWon++;
                string[] args = stat.Split(", ");
                Points += Int32.Parse(args[0]); // adding to Points
                ScoreDiff += Int32.Parse(args[1]); // adding to ScoreDiff
            }

            public override void Withdraw()
            {
                base.Withdraw();
                Points = 0;
                ScoreDiff = 0;
                for (int i = 0; i < MatchesPlayed; i++)
                    ScoreDiff -= 42;

            }

            public override string GetStats()
            {
                return Points + ", " + MatchesWon + ", " + ScoreDiff;
            }
        }
    }
}
