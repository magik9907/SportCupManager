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
                this.Name = name;
                this.PlayersEliminated = 0;
                this.SumOfPlayersLeft = 0;
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

            public override void SetMatchResult(bool result, string stat)
            {
                this.MatchesPlayed++;
                if (result)
                {
                    this.MatchesWon++;
                    SumOfPlayersLeft += Int32.Parse(stat);
                }
                else
                    PlayersEliminated += Int32.Parse(stat);
            }

            public override string GetStats()
            {
                return MatchesWon + ", " + PlayersEliminated + ", " + SumOfPlayersLeft;
            }
        }
    }
}
