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
            public float SumWinTime { get; set; }
            public float SumLossTime { get; set; }

            public TugOfWarTeam(string name,int id) : base(name,id)
            {
                this.AvWinTime = 0;
                this.AvLossTime = 0;
                this.SumWinTime = 0;
                this.SumLossTime = 0;
            }
            
            public TugOfWarTeam(string name, int id, List<TPerson.Player> players) : this(name, id)
            {
                this.listPlayers.AddRange(players);
            }

            public static bool operator <(TugOfWarTeam a, TugOfWarTeam b)
            {
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon < b.MatchesWon;
                if (a.AvWinTime != b.AvWinTime)
                    return a.AvWinTime < b.AvWinTime;
                if (a.AvLossTime != b.AvLossTime)
                    return a.AvLossTime < b.AvLossTime;
                return String.Compare(a.Name, b.Name) > 0;
            }

            public static bool operator >(TugOfWarTeam a, TugOfWarTeam b)
            {
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon > b.MatchesWon;
                if (a.AvWinTime != b.AvWinTime)
                    return a.AvWinTime > b.AvWinTime;
                if (a.AvLossTime != b.AvLossTime)
                    return a.AvLossTime > b.AvLossTime;
                return String.Compare(a.Name, b.Name) < 0;
            }

            public override void SetMatchResult(bool result, bool wasPlayedBefore, bool wasWinner, string stat)
            {
                if(!wasPlayedBefore)
                    this.MatchesPlayed++;
                if (result)
                {
                    if(!wasWinner)
                        this.MatchesWon++;
                    this.SumWinTime += float.Parse(stat);
                }
                else
                {
                    this.SumLossTime += float.Parse(stat);
                }
                    
            }

            public override void Withdraw()
            {
                base.Withdraw();
                SumLossTime = 0;
                SumWinTime = 0;
                for (int i = 0; i < MatchesPlayed; i++)
                    SumLossTime += 5;

            }

            public override string GetStats()
            {
                return MatchesWon + ", " + AvWinTime + ", " + AvLossTime;
            }
        }
    }
}
