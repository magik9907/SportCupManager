using System;

namespace TournamentManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            ITournament t = new Tournament("Name",(int)TEnum.TournamentDyscypline.dodgeball);
            t.AddReferee(new TPerson.Referee("ff", "ff", 1));
            t.AddTeam(new TTeam.DodgeballTeam("44"));
            t.AddReferee(new TPerson.Referee("ff", "cc", 1));
            t.AddTeam(new TTeam.DodgeballTeam("33"));
            t.AddReferee(new TPerson.Referee("ff", "bb", 1));
            t.AddTeam(new TTeam.DodgeballTeam("22"));
            t.AddReferee(new TPerson.Referee("ff", "aa", 1));
            t.AddTeam(new TTeam.DodgeballTeam("11"));
            Save.Tournament(t);
        }
    }
}
