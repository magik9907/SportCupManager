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
            t.AddReferee(new TPerson.Referee("i", "i", 1));
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
            t.SetAutoLeague();
            for(int i = 0; i < t.League.Teams.Count; i++)
                for(int j = i + 1; j < t.League.Teams.Count; j++)
                    t.League.SetResult(t.League.Teams[i], t.League.Teams[j], t.League.Teams[i].Name + ": 21, 21, 0. " + t.League.Teams[j].Name + ": 0, 0, 0");
            

            Save.Tournament(t);
        }
    }
}
