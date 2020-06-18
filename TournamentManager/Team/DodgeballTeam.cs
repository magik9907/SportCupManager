using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace TournamentManager
{
    namespace TTeam
    {
        public class DodgeballTeam : Team
        {
            public int PlayersEliminated { get; set; }
            public int SumOfPlayersLeft { get; set; }

            public DodgeballTeam(string name,int id) : base(name,id)
            {
                this.PlayersEliminated = 0;
                this.SumOfPlayersLeft = 0;
            }

            public DodgeballTeam(string name, int id, List<TPerson.Player> players) : this(name, id)
            {
                this.listPlayers.AddRange(players);
            }

            public DodgeballTeam(string name, int id, List<TPerson.Player> players, Dictionary<string, float> stats) : this(name, id,players)
            {
                PlayersEliminated = (int)Math.Round(stats["PlayersEliminated"]);
                SumOfPlayersLeft= (int)Math.Round(stats["SumOfPlayersLeft"]);
                MatchesPlayed = (int)Math.Round(stats["MatchesPlayed"]);
                MatchesWon = (int)Math.Round(stats["MatchesWon"]);
            }

            public static bool operator <(DodgeballTeam a, DodgeballTeam b)
            {
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon < b.MatchesWon;
                if (a.PlayersEliminated != b.PlayersEliminated)
                    return a.PlayersEliminated < b.PlayersEliminated;
                if (a.SumOfPlayersLeft != b.SumOfPlayersLeft)
                    return a.SumOfPlayersLeft < b.SumOfPlayersLeft;
                return String.Compare(a.Name, b.Name) > 0;
            }

            public static bool operator >(DodgeballTeam a, DodgeballTeam b)
            {
                if (a.MatchesWon != b.MatchesWon)
                    return a.MatchesWon > b.MatchesWon;
                if (a.PlayersEliminated != b.PlayersEliminated)
                    return a.PlayersEliminated > b.PlayersEliminated;
                if (a.SumOfPlayersLeft != b.SumOfPlayersLeft)
                    return a.SumOfPlayersLeft > b.SumOfPlayersLeft;
                return String.Compare(a.Name, b.Name) < 0;
            }

            //new string format: "Players left, players eliminated"
            public override void SetMatchResult(bool result, bool wasPlayedBefore, bool wasWinner, string stat)
            {
                string[] tmp = stat.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                if(!wasPlayedBefore)
                    this.MatchesPlayed++;
                if (result && !wasWinner)
                    this.MatchesWon++;
                if (!result && wasWinner)
                    this.MatchesWon--;
                SumOfPlayersLeft += Int32.Parse(tmp[0]);
                PlayersEliminated += Int32.Parse(tmp[1]);
                
            }

            public override void Withdraw()
            {
                base.Withdraw();
                PlayersEliminated = 0;
                SumOfPlayersLeft = 0;
                PlayersEliminated = 0;
            }

            public override string GetStats()
            {
                return MatchesWon + ", " + PlayersEliminated + ", " + SumOfPlayersLeft;
            }
        }
    }
}
