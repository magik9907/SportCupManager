using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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
        /// <param name="date">start date of first round</param>
        /// <param name="space">space between rounds</param>
        void SetAutoLeague(int[] date, int space);

        /// <summary>
        /// Creating playoff round 
        /// </summary>
        /// <param name="teams">list of teams allow to play in competition</param>
        void SetPlayOff(int[] date);

        /// <summary>
        /// adding referee to the tournament
        /// </summary>
        /// <param name="referee"> referee object to delete</param>
        void AddReferee(TPerson.Referee referee);

        /// <summary>
        /// adding referee to the tournament
        /// </summary>
        /// <param name="referee">list of referees object to delete</param>
        void AddReferee(List<TPerson.Referee> referees);

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
        /// find team using name
        /// </summary>
        /// <param name="name">name of team to find</param>
        /// <returns></returns>
        TTeam.Team FindTeam(string name);
        
        
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
        /// return enum dyscypline of tournament
        /// </summary>
        TEnum.TournamentDyscypline Dyscypline
        {
            get;
        }

        /// <summary>
        /// return league round
        /// </summary>
        TRound.League League
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

        /// <summary>
        /// return playoff round object
        /// </summary>
        TRound.PlayOff PlayOff { get; }
    }

    /// <summary>
    /// Main class of TournamentManager
    /// It can create league and playoff, add teams and referees to tournament
    /// </summary>
    public class Tournament : ITournament
    {
        [JsonProperty("Dyscypline")]
        /// <summary>
        /// contain tournament dyscipline id
        /// </summary>
        private readonly TEnum.TournamentDyscypline dyscypline;

        [JsonProperty("Name")]
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
        public Tournament(string name, TEnum.TournamentDyscypline dyscypline = 0)
        {
            if (string.IsNullOrEmpty(name) && Enum.GetName(typeof(TEnum.TournamentDyscypline), dyscypline) == "undifined")
                TNDException();
            if (string.IsNullOrEmpty(name))
                TDException("tournament name");
            if (Enum.GetName(typeof(TEnum.TournamentDyscypline), dyscypline) == "undifined")
                TDException("tournament dyscypline");
            this.name = name;
            this.dyscypline = dyscypline;

        }

        public void AddReferee(TPerson.Referee referee = null)
        {
            IsObjectNotDefined(referee, "Referee");
            referees.Add(referee);
        }

        public void AddReferee(List<TPerson.Referee> referees = null)
        {
            IsObjectNotDefined(referees, "Referee");
            this.referees.AddRange(referees);
        }

        public void AddTeam(TTeam.ITeam team = null)
        {
            IsObjectNotDefined(team, "ITeam");
            teams.Add(team);
        }

        public void RemoveReferee(TPerson.Referee referee)
        {
            IsObjectNotDefined(referee, "Referee");
            referees.Remove(referee);
        }

        public void RemoveTeam(TTeam.ITeam team)
        {
            IsObjectNotDefined(team, "Iteam");
            teams.Remove(team);
        }

        public void SetAutoLeague(int[] date, int space)
        {
            CheckNumberOfTeams(teams, 5);
            league = new TRound.League(teams, referees);
            league.AutoSchedule(date, space);
        }

        public void SetPlayOff(int[] date)
        {

            List<TTeam.ITeam> teams = league.GetFinalTeams(4);
            CheckNumberOfTeams(league.GetFinalTeams(4), 4);
            playoff = new TRound.PlayOff(teams, Referees, date);
        }

        public TTeam.Team FindTeam(string name)
        {
            foreach (TTeam.Team team in Teams)
            {
                if (team.Name == name) return team;
            }
            return null;
        }


        [JsonIgnore]
        public List<TPerson.Referee> Referees
        {
            get
            {
                return referees;
            }
        }

        [JsonIgnore]
        public List<TTeam.ITeam> Teams
        {
            get
            {
                return teams;
            }
        }

        [JsonIgnore]
        public TEnum.TournamentDyscypline Dyscypline
        {
            get
            {
                return dyscypline;
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return name;
            }
        }

        [JsonIgnore]
        public TRound.League League
        {
            get
            {
                return league;
            }
        }
        [JsonIgnore]
        public TRound.PlayOff PlayOff
        {
            get
            {
                return playoff;
            }
        }

        private void IsObjectNotDefined(List<Object> obj, string objType)
        {
            if (obj == null)
            {
                throw new TException.ObjectNotDefined(objType);
            }
        }

        /// <summary>
        /// check if object is defined
        /// </summary>
        /// <param name="obj">reference to object</param>
        /// <param name="objType">type of object</param>
        /// 
        private void IsObjectNotDefined(Object obj, string objType)
        {
            if (obj == null)
            {
                throw new TException.ObjectNotDefined(objType);
            }
        }

        /// <summary>
        /// check if number of teams is equal or greather to param "limit"
        /// </summary>
        /// <param name="ts">list of teams</param>
        /// <param name="limit"> minimal number of teams to play</param>
        private void CheckNumberOfTeams(List<TTeam.ITeam> ts, int limit)
        {
            if (ts.Count < limit)
                throw new TException.NotEnoughtTeamsNumber(ts.Count);
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
