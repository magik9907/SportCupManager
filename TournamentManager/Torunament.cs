using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TournamentManager.TPerson;
using TournamentManager.TTeam;
/// <summary>
///     main name space 
/// </summary>
namespace TournamentManager
{
    /// <summary>
    /// Tournament interface
    /// </summary>
    public interface ITournament
    {
        /// <summary>
        /// Create league type from Team list
        /// </summary>
        void SetLeague();
        /// <summary>
        /// Creating playoff round 
        /// </summary>
        /// <param name="teams">list of teams allow to play in competition</param>
        void SetPlayOff(List<TTeam.ITeam> teams);
        /// <summary>
        /// adding referee to the tournament
        /// </summary>
        /// <param name="referee"> referee object to delete</param>
        void AddReferee(TPerson.Referee referee);
        /// <summary>
        /// remove referee
        /// </summary>
        /// <param name="referee">referee object to delete</param>
        void RemoveReferee(TPerson.Referee referee);
        /// <summary>
        /// add team to competition
        /// </summary>
        /// <param name="team">team object</param>
        void AddTeam(TTeam.ITeam team);
        /// <summary>
        /// remove team from competition
        /// </summary>
        /// <param name="team">team object</param>
        void RemoveTeam(TTeam.ITeam team);
        /// <summary>
        /// return tournament mame;
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// return dyscypline of tournament
        /// </summary>
        string Dyscypline
        {
            get;
        }
        /// <summary>
        /// return list of referees
        /// </summary>
        List<TPerson.Referee> Referees
        {
            get;
        }
        /// <summary>
        /// return List of teams
        /// </summary>
        List<TTeam.ITeam> Teams
        {
            get;
        }
    }
    /// <summary>
    /// Main class of TournamentManager
    /// 
    /// </summary>
    public class Tournament : ITournament
    {

        /// <summary>
        /// contain tournament dyscipline
        /// </summary>
        private readonly string dyscypline;
        /// <summary>
        /// contain name of tournament
        /// </summary>
        private readonly string name;
        /// <summary>
        /// contain league object
        /// </summary>
        private TRound.League league;
        /// <summary>
        /// contains playoff object
        /// </summary>
        private TRound.PlayOff playoff;
        /// <summary>
        /// list of referees allow to umpire in tournament
        /// </summary>
        private List<TPerson.Referee> referees = new List<TPerson.Referee>();
        /// <summary>
        /// list of team taking part in competiton
        /// </summary>
        private List<TTeam.ITeam> teams = new List<TTeam.ITeam>();
        /// <summary>
        /// invalid constructor
        /// </summary>
        public Tournament()
        {
            TNDException();
        }
        /// <summary>
        /// invalid constuctor
        /// </summary>
        /// <param name="name"> contains name of tournament</param>
        public Tournament(string name)
        {
            TDException("tournament dyscypline");
        }
        /// <summary>
        /// contructor of tournament 
        /// </summary>
        /// <param name="name">name of tournament</param>
        /// <param name="dyscypline">tournament discipline</param>
        public Tournament(string name, string dyscypline)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(dyscypline))
                TNDException();
            if (string.IsNullOrEmpty(name))
                TDException("tournament name");
            if (string.IsNullOrEmpty(dyscypline))
                TDException("tournament dyscypline");
            this.name = name;
            this.dyscypline = dyscypline;

        }
        public void AddReferee(TPerson.Referee referee = null)
        {
            IsObjectNotDefined(referee, "Referee");
            referees.Add(referee);
        }

        public void AddTeam(ITeam team = null)
        {
            IsObjectNotDefined(team,"ITeam");
            teams.Add(team);
        }

        public void RemoveReferee(TPerson.Referee referee)
        {
            IsObjectNotDefined(referee, "Referee");
            referees.Remove(referee);
        }

        public void RemoveTeam(ITeam team)
        {
            IsObjectNotDefined(team,"Iteam");
            teams.Remove(team);
        }

        public void SetLeague()
        {
            CheckNumberOfTeams(teams);
            league = new TRound.League(teams, referees);
        }

        public void SetPlayOff(List<ITeam> teams)
        {
            CheckNumberOfTeams(teams);
            playoff = new TRound.PlayOff(teams, referees);
        }
        /// <summary>
        /// return list of referees
        /// </summary>
        public List<TPerson.Referee> Referees
        {
            get{
                return referees;
            }
        }
        public List<TTeam.ITeam> Teams
        {
            get
            {
                return teams;
            }
        }
        public string Dyscypline
        {
            get
            {
                return dyscypline;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// check if object is defined
        /// </summary>
        /// <param name="obj">reference to object</param>
        /// <param name="objType">type of object</param>
        private void IsObjectNotDefined(Object obj, string objType)
        {
            if(obj == null)
            {
                throw new TException.ObjectNotDefined(objType);
            }
        }
        /// <summary>
        /// check if number of teams is greather than 1
        /// </summary>
        /// <param name="ts">list of teams</param>
        private void CheckNumberOfTeams(List<TTeam.ITeam> ts)
        {
            if (teams.Count < 2)
                throw new TException.NotEnoughtTeamsNumber(teams.Count);
        }
        /// <summary>
        /// throw data exception ex:tournament dyscipline 
        /// </summary>
        /// <param name="message">type of data exception </param>
        private void TDException(string message)
        {
            throw new TException.TournamentDataException(message);
        }
        /// <summary>
        /// no data given to create tournamet
        /// </summary>
        private void TNDException()
        {
            throw new TException.TournamentMustBeDefine(); 
        }
    }

}
