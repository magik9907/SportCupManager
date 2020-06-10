using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TournamentManager
{
    namespace TTeam
    {
        public interface ITeam
        {
            public void addPlayer(TPerson.Player p);
            public void removePlayer(TPerson.Player p);
            public string ToString();
        }
    }
}
