using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SportCupManagerConsole
{
    interface ITeam
    {
        public void addPlayer(Player p);
        public void removePlayer(Player p);
        public string ToString();
    }
}
