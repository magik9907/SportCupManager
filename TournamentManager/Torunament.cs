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
    interface ITournament
    {
        /// <summary>
        /// Create league type from Team list
        /// </summary>
        void SetLeague();
        /// <summary>
        /// Creating playoff round 
        /// </summary>
        /// <param name="teams">list of teams allow to play in competition</param>
        void SetPlayOff(List <TTeam.ITeam> teams);
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
    }
    /// <summary>
    /// Main class of TournamentManager
    /// 
    /// </summary>
    public class Tournament : ITournament
    {
        private string typeOfTournament;
        private string name;
        private TRound.League league;
        private TRound.PlayOff playoff;
        private List<TPerson.Referee> listReferees = new List<TPerson.Referee>();
        private List<TTeam.ITeam> teams = new List<TTeam.ITeam>();

        public Tournament()
        {
            TNDException();
        }

        public Tournament(string name)
        {
            TDException("tournament name");
        }

        public Tournament(string name, string type)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type))
                TNDException();
            if (string.IsNullOrEmpty(name))
                TDException("tournament name");
            if (string.IsNullOrEmpty(type))
                TDException("tournament type");
            this.name = name;
            this.typeOfTournament = type;

        }

        public void AddReferee(Referee referee)
        {

        }

        public void AddTeam(ITeam team)
        {

        }

        public void RemoveReferee(Referee referee)
        {

        }

        public void RemoveTeam(ITeam team)
        {

        }

        public void SetLeague()
        {

        }

        public void SetPlayOff(List<ITeam> teams)
        {

        }

        public string Type
        {
            get
            {
                return typeOfTournament;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        private void TDException(string message)
        {
            throw new TException.TournamentDataException(message);
        }

        private void TNDException()
        {
            throw new TException.TournamentMustBeDefine(); 
        }
    }

}
