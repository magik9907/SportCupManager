using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    interface ITournament
    {
    + setLeague()
+ setPlayOff(teams:List<ITeam>)
+ addReferee(r:Referee)
+ removeReferee(r: Referee)
+ addTeam(t:ITeam)
+ removeTeam(t:I Team)
    }

    class Tournament : ITournament
    {

    }

}
