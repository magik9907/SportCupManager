using System;
using System.Collections.Generic;
using System.Text;
using TournamentManager.TPerson;

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

            public TugOfWarTeam(string name, int id, List<TPerson.Player> players, Dictionary<string, float> stats) : this(name, id,players)
            {
                AvWinTime = stats["AvWinTime"];
                AvLossTime = stats["AvLossTime"];
                SumWinTime = stats["SumWinTime"];
                SumLossTime = stats["SumLossTime"];
                MatchesPlayed = (int)Math.Round(stats["MatchesPlayed"]);
                MatchesWon = (int)Math.Round(stats["MatchesWon"]);
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

            //string format "newTime, oldTime"
            public override void SetMatchResult(bool result, bool wasPlayedBefore, bool wasWinner, string stat)
            {
                string[] tmp = stat.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                if(!wasPlayedBefore)
                    this.MatchesPlayed++;
                if (result)
                {
                    if (!wasWinner)
                    {
                        this.MatchesWon++;
                        this.SumLossTime -= float.Parse(tmp[1]);
                    }
                    else
                        this.SumWinTime -= float.Parse(tmp[1]);
                    this.SumWinTime += float.Parse(tmp[0]);
                }
                else
                {
                    if (wasWinner)
                    {
                        this.MatchesWon--;
                        this.SumWinTime -= float.Parse(tmp[1]);
                    }
                    else
                        this.SumLossTime -= float.Parse(tmp[1]);
                    this.SumLossTime += float.Parse(tmp[0]);
                }
                SetAverages();
            }

            private void SetAverages()
            {
                if (MatchesWon > 0)
                    AvWinTime = SumWinTime / MatchesWon;
                else
                    AvWinTime = 0;
                if (MatchesPlayed > MatchesWon)
                    AvLossTime = SumLossTime / (MatchesPlayed - MatchesWon);
                else
                    AvLossTime = 0;
            }

            public override void Withdraw()
            {
                base.Withdraw();
                SumLossTime = 0;
                SumWinTime = 0;
            }

            public override string GetStats()
            {
                SetAverages();
                return MatchesWon + ", " + AvWinTime + ", " + AvLossTime;
            }
        }
    }
}
