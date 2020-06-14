﻿using System;
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
                this.Name = name;
                this.AvWinTime = 0;
                this.AvLossTime = 0;
                this.SumWinTime = 0;
                this.SumLossTime = 0;
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

            public override void SetMatchResult(bool result, string stat)
            {
                this.MatchesPlayed++;
                if (result)
                {
                    this.MatchesWon++;
                    this.SumWinTime += float.Parse(stat);
                }
                else
                {
                    this.SumLossTime += float.Parse(stat);
                }
                    
            }

            public override string GetStats()
            {
                return MatchesWon + ", " + AvWinTime + ", " + AvLossTime;
            }
        }
    }
}
