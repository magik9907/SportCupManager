﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
﻿using TournamentManager.TPerson;

namespace TournamentManager
{
    namespace TTeam
    {
        public interface ITeam<T>
        {
            string Name { get; set; }

            void AddPlayer(Player p);
            bool LessThan(T a, T b);
            bool GreaterThan(T a, T b);
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
