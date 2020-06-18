using TournamentManager.TPerson;

namespace TournamentManager
{
    namespace TTeam
    {
        public interface ITeamId
        {
            int Id { get; }
        }

        public interface ITeam
        {
            string Name { get; set; }
            bool DidWithdraw { get; }
            void AddPlayer(Player p);
            void RemovePlayer(Player p);
            void Withdraw();
            void SetMatchResult(bool result, bool wasPlayedBefore, bool wasWinner, string stat);
            //format stat for DodgeballTeam: "PlayersLeft"
            //format stat for VolleyballTeam: "Points, ScoreDiff"
            //format stat for TugOfWarTeam: "MatchTime"
            string GetStats();
            string ToString();
        }
    }
}
