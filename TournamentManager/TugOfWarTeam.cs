using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TTeam
    {
        public class TugOfWarTeam : Team
        {
            public float AvWinTime { get; set; }
            public float AvLossTime { get; set; }

            public TugOfWarTeam(string name) : base(name)
            {
                this.Name = name;
            }

            public static bool operator <(TugOfWarTeam a, TugOfWarTeam b)
            {
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon < b.MatchesWon;
                if (a.AvWinTime != b.AvWinTime)
                    return a.AvWinTime < b.AvWinTime;
                if (a.AvLossTime != b.AvLossTime)
                    return a.AvLossTime < b.AvLossTime;
                return String.Compare(a.Name, b.Name) < 0;
            }

            public static bool operator >(TugOfWarTeam a, TugOfWarTeam b)
            {
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon > b.MatchesWon;
                if (a.AvWinTime != b.AvWinTime)
                    return a.AvWinTime > b.AvWinTime;
                if (a.AvLossTime != b.AvLossTime)
                    return a.AvLossTime > b.AvLossTime;
                return String.Compare(a.Name, b.Name) > 0;
            }
        }
    }
}
