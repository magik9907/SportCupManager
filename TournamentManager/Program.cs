using System;

namespace TournamentManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            ITournament t = new Tournament("Name",(int)TEnum.TournamentDyscypline.dodgeball);
            t.AddReferee(new TPerson.Referee("a", "a", 1));
            t.AddReferee(new TPerson.Referee("b", "b", 1));
            t.AddReferee(new TPerson.Referee("c", "c", 1));
            t.AddReferee(new TPerson.Referee("d", "d", 1));
            t.AddReferee(new TPerson.Referee("e", "e", 1));
            t.AddTeam(new TTeam.DodgeballTeam("11"));
            t.AddTeam(new TTeam.DodgeballTeam("22"));
            t.AddTeam(new TTeam.DodgeballTeam("33"));
            t.AddTeam(new TTeam.DodgeballTeam("44"));
            t.AddTeam(new TTeam.DodgeballTeam("55"));
            t.AddTeam(new TTeam.DodgeballTeam("66"));

            t.SetAutoLeague();

            Save.Tournament(t);
        }
    }
}
