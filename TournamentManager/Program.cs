using System;
using System.Globalization;

namespace TournamentManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            ITournament t = new Tournament("volley", (int)TEnum.TournamentDyscypline.volleyball);
            t.AddReferee(new TPerson.Referee("a", "a", 1));
            t.AddReferee(new TPerson.Referee("b", "b", 1));
            t.AddReferee(new TPerson.Referee("c", "c", 1));
            t.AddReferee(new TPerson.Referee("d", "d", 1));
            t.AddReferee(new TPerson.Referee("e", "e", 1));
            t.AddReferee(new TPerson.Referee("f", "f", 1));
            t.AddReferee(new TPerson.Referee("g", "g", 1));
            t.AddReferee(new TPerson.Referee("h", "h", 1));
            t.AddReferee(new TPerson.Referee("i", "j", 1));
            t.AddReferee(new TPerson.Referee("j", "k", 1));
            TTeam.ITeam te1 = new TTeam.VolleyballTeam("11");
            te1.AddPlayer(new TPerson.Player("1", "1", 1, 1));
            te1.AddPlayer(new TPerson.Player("2", "2", 1, 1));
            te1.AddPlayer(new TPerson.Player("3", "3", 1, 1));
            te1.AddPlayer(new TPerson.Player("4", "4", 1, 1));
            t.AddTeam(te1);
            TTeam.ITeam te2 = new TTeam.VolleyballTeam("22");
            te2.AddPlayer(new TPerson.Player("1", "1", 1, 1));
            te2.AddPlayer(new TPerson.Player("2", "2", 1, 1));
            te2.AddPlayer(new TPerson.Player("3", "3", 1, 1));
            te2.AddPlayer(new TPerson.Player("4", "4", 1, 1));
            t.AddTeam(te2);
            t.AddTeam(new TTeam.VolleyballTeam("33"));
            t.AddTeam(new TTeam.VolleyballTeam("44"));
            t.AddTeam(new TTeam.VolleyballTeam("55"));
            t.AddTeam(new TTeam.VolleyballTeam("66"));



            /*
            ITournament t = new Tournament("dodge", (int)TEnum.TournamentDyscypline.dodgeball);
            TTeam.ITeam te = new TTeam.DodgeballTeam("11");
            t.AddReferee(new TPerson.Referee("a", "a", 1));
            t.AddReferee(new TPerson.Referee("b", "b", 1));
            t.AddReferee(new TPerson.Referee("c", "c", 1));
            t.AddReferee(new TPerson.Referee("d", "d", 1));
            t.AddReferee(new TPerson.Referee("e", "e", 1));
            t.AddReferee(new TPerson.Referee("f", "f", 1));
            t.AddReferee(new TPerson.Referee("g", "g", 1));
            t.AddReferee(new TPerson.Referee("h", "h", 1));
            t.AddReferee(new TPerson.Referee("i", "j", 1));
            t.AddReferee(new TPerson.Referee("j", "k", 1));
            te.AddPlayer(new TPerson.Player("1", "1", 1, 1));
            te.AddPlayer(new TPerson.Player("2", "2", 1, 1));
            te.AddPlayer(new TPerson.Player("3", "3", 1, 1));
            te.AddPlayer(new TPerson.Player("4", "4", 1, 1));
            t.AddTeam(te);
            t.AddTeam(new TTeam.DodgeballTeam("11"));
            t.AddTeam(new TTeam.DodgeballTeam("22"));
            t.AddTeam(new TTeam.DodgeballTeam("33"));
            t.AddTeam(new TTeam.DodgeballTeam("44"));
            t.AddTeam(new TTeam.DodgeballTeam("55"));
            t.AddTeam(new TTeam.DodgeballTeam("66"));
            */
            /*
             
            ITournament t = new Tournament("tugofwar", (int)TEnum.TournamentDyscypline.tugofwar);
            TTeam.ITeam te = new TTeam.TugOfWarTeam("11");
            t.AddReferee(new TPerson.Referee("a", "a", 1));
            t.AddReferee(new TPerson.Referee("b", "b", 1));
            t.AddReferee(new TPerson.Referee("c", "c", 1));
            t.AddReferee(new TPerson.Referee("d", "d", 1));
            t.AddReferee(new TPerson.Referee("e", "e", 1));
            t.AddReferee(new TPerson.Referee("f", "f", 1));
            t.AddReferee(new TPerson.Referee("g", "g", 1));
            t.AddReferee(new TPerson.Referee("h", "h", 1));
            t.AddReferee(new TPerson.Referee("i", "j", 1));
            t.AddReferee(new TPerson.Referee("j", "k", 1));
            te.AddPlayer(new TPerson.Player("1", "1", 1, 1));
            te.AddPlayer(new TPerson.Player("2", "2", 1, 1));
            te.AddPlayer(new TPerson.Player("3", "3", 1, 1));
            te.AddPlayer(new TPerson.Player("4", "4", 1, 1));
            t.AddTeam(te);
            t.AddTeam(new TTeam.TugOfWarTeam("11"));
            t.AddTeam(new TTeam.TugOfWarTeam("22"));
            t.AddTeam(new TTeam.TugOfWarTeam("33"));
            t.AddTeam(new TTeam.TugOfWarTeam("44"));
            t.AddTeam(new TTeam.TugOfWarTeam("55"));
            t.AddTeam(new TTeam.TugOfWarTeam("66"));
             */
            t.SetAutoLeague(new int[] { 1, 1, 1 }, 1);
            // t.SetPlayOff(new int[] { 1, 1, 1 });
            Save.Tournament(t);
        }
    }
}
