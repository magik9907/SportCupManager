﻿using System;
using System.Globalization;

namespace TournamentManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
            TTeam.ITeam te1;
            ITournament t = null;
            int i;
           
                    t = new Tournament("volley", TEnum.TournamentDyscypline.volleyball);
                    i = 1;
                    TPerson.Referee r = new TPerson.Referee("a", "a", 1, i);
                    t.AddReferee(r);
                    i++;
                    t.AddReferee(new TPerson.Referee("b", "b", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("c", "c", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("d", "d", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("e", "e", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("f", "f", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("g", "g", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("h", "h", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("i", "j", 1, i));
                    i++;
                    t.AddReferee(new TPerson.Referee("j", "k", 1, i));
                    i = 1;
                    te1 = new TTeam.VolleyballTeam("11", i);
                    te1.AddPlayer(new TPerson.Player("1", "1", 1, 1));
                    te1.AddPlayer(new TPerson.Player("2", "2", 1, 1));
                    te1.AddPlayer(new TPerson.Player("3", "3", 1, 1));
                    te1.AddPlayer(new TPerson.Player("4", "4", 1, 1));
                    t.AddTeam(te1);
                    i++;
                    TTeam.ITeam te2 = new TTeam.VolleyballTeam("22", i);
                    te2.AddPlayer(new TPerson.Player("1", "1", 1, 1));
                    te2.AddPlayer(new TPerson.Player("2", "2", 1, 1));
                    te2.AddPlayer(new TPerson.Player("3", "3", 1, 1));
                    te2.AddPlayer(new TPerson.Player("4", "4", 1, 1));
                    t.AddTeam(te2);
                    i++;
                    t.AddTeam(new TTeam.VolleyballTeam("33", i));
                    i++;
                    t.AddTeam(new TTeam.VolleyballTeam("44", i));
                    i++;
                    t.AddTeam(new TTeam.VolleyballTeam("55", i));
                    i++;
                    t.AddTeam(new TTeam.VolleyballTeam("66", i));
            t.SetAutoLeague(new int[] { 1, 1, 1 }, 1);
                    for (i = 0; i < t.League.Teams.Count; i++)
                        for (int j = t.League.Teams.Count - 1; j > i; j--)
                            t.League.SetResult(t.League.Teams[i].Name + ": 21, 21, 0. " + t.League.Teams[j].Name + ": 0, 0, 0", t.League.Teams[i], t.League.Teams[j]);
                    
                    t.SetPlayOff(new int[] { 1, 1, 1 });
                    Console.WriteLine(t.PlayOff.GetWinner());
                   Save.Tournament(t);
                    ITournament newT;
                    newT = Read.Tournament("volley");
                    Console.WriteLine(newT.Name);
                    Console.WriteLine(newT.Dyscypline);
            */
            

            
            /*
            ITournament t = new Tournament("dodge", TEnum.TournamentDyscypline.dodgeball);

                    int i =1;


                    t.AddReferee(new TPerson.Referee("a", "a", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("b", "b", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("c", "c", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("d", "d", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("e", "e", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("f", "f", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("g", "g", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("h", "h", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("i", "j", 1,i));
                    i++;
                    t.AddReferee(new TPerson.Referee("j", "k", 1,i));
                    i = 1;
                    TTeam.ITeam te = new TTeam.DodgeballTeam("11",i);

                    te.AddPlayer(new TPerson.Player("1", "1", 1, 1));
                    te.AddPlayer(new TPerson.Player("2", "2", 1, 1));
                    te.AddPlayer(new TPerson.Player("3", "3", 1, 1));
                    te.AddPlayer(new TPerson.Player("4", "4", 1, 1));
                    t.AddTeam(te);
                    i++;
                    t.AddTeam(new TTeam.DodgeballTeam("11",i));
                    i++;
                    t.AddTeam(new TTeam.DodgeballTeam("22",i));
                    i++;
                    t.AddTeam(new TTeam.DodgeballTeam("33",i));
                    i++;
                    t.AddTeam(new TTeam.DodgeballTeam("44",i));
                    i++;
                    t.AddTeam(new TTeam.DodgeballTeam("55",i));
                    i++;
                    t.AddTeam(new TTeam.DodgeballTeam("66",i));
            t.SetAutoLeague(new int[] { 1, 1, 1 }, 1);
            for (i = 0; i < t.League.Teams.Count; i++)
                for (int j = t.League.Teams.Count - 1; j > i; j--)
                    t.League.SetResult("4", t.League.Teams[i], t.League.Teams[j]);
            t.League.WithdrawTeam(te);
            t.SetPlayOff(new int[] { 1, 1, 1 });
            Console.WriteLine(t.PlayOff.GetWinner());
            Save.Tournament(t);
            ITournament newT;
            newT = Read.Tournament("dodge");
            Console.WriteLine(newT.Name);
            Console.WriteLine(newT.Dyscypline);
            */
            

            ITournament t = new Tournament("tugofwar", TEnum.TournamentDyscypline.tugofwar);
            int i = 1;
            t.AddReferee(new TPerson.Referee("a", "a", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("b", "b", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("c", "c", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("d", "d", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("e", "e", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("f", "f", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("g", "g", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("h", "h", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("i", "j", 1,i));
            i++;
            t.AddReferee(new TPerson.Referee("j", "k", 1,i));
            i = 1;

            TTeam.ITeam te = new TTeam.TugOfWarTeam("11",i);

            te.AddPlayer(new TPerson.Player("1", "1", 1, 1));
            te.AddPlayer(new TPerson.Player("2", "2", 1, 1));
            te.AddPlayer(new TPerson.Player("3", "3", 1, 1));
            te.AddPlayer(new TPerson.Player("4", "4", 1, 1));
            t.AddTeam(te);
            i++;
            t.AddTeam(new TTeam.TugOfWarTeam("11",i));
            i++;
            t.AddTeam(new TTeam.TugOfWarTeam("22",i));
            i++;
            t.AddTeam(new TTeam.TugOfWarTeam("33",i));
            i++;
            t.AddTeam(new TTeam.TugOfWarTeam("44",i));
            i++;
            t.AddTeam(new TTeam.TugOfWarTeam("55",i));
            i++;
            t.AddTeam(new TTeam.TugOfWarTeam("66",i));

             t.SetAutoLeague(new int[] { 1, 1, 1 }, 1);
                    for (i = 0; i < t.League.Teams.Count; i++)
                        for (int j = t.League.Teams.Count - 1; j > i; j--)
                            t.League.SetResult("782,21", t.League.Teams[i], t.League.Teams[j]);
                   t.League.WithdrawTeam(te);
                    t.SetPlayOff(new int[] { 1, 1, 1 });
                    t.PlayOff.SetResult("1,3456", t.PlayOff.Rounds[0].ListMatches[0].TeamA);
                    t.PlayOff.SetResult("1,3456", t.PlayOff.Rounds[0].ListMatches[1].TeamA);
                    t.PlayOff.SetResult("1,3456", t.PlayOff.Rounds[1].ListMatches[0].TeamA);
            Console.WriteLine(t.PlayOff.GetWinner());
                    Save.Tournament(t);
                    ITournament newT;
                    newT = Read.Tournament("tugofwar");
                    Console.WriteLine(newT.Name);
                    Console.WriteLine(newT.Dyscypline);
            


        }
    }
    }

