using TournamentManager.TPerson;

namespace TournamentManager
{
    namespace TTeam
    {
        public interface ITeam
        {
            string Name { get; set; }

            void AddPlayer(Player p);
            void RemovePlayer(Player p);
            void SetMatchResult(bool result, string stat);
            //format stat for DodgeballTeam: "PlayersLeft"
            //format stat for VolleyballTeam: "Points, ScoreDiff"
            //format stat for TugOfWarTeam: "MatchTime"
            string GetStats();
            string ToString();
        }
    }
}
