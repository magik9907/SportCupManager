using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    class TugOfWarTeam : Team
    {
        private float avWinTime;
        private float avLossTime;

        public TugOfWarTeam(string name) : base(name)
        {
            this.name = name;
        }

        public static bool operator <(TugOfWarTeam a, TugOfWarTeam b)
        {
            return (a.matchesWon < b.matchesWon) ? true : false;
        }

        public static bool operator >(TugOfWarTeam a, TugOfWarTeam b)
        {
            return (a.matchesWon > b.matchesWon) ? true : false;
        }

        public void setAvWinTime(int minutes, int seconds)
        {

        }

        public void 
    }
}
