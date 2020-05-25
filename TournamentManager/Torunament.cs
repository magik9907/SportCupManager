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
        void setLeague();
        /// <summary>
        /// Creating playoff round 
        /// </summary>
        /// <param name="teams">list of teams allow to play in competition</param>
        void setPlayOff(List <TTeam.ITeam> teams);
        /// <summary>
        /// adding referee to the tournament
        /// </summary>
        /// <param name="referee"> referee object to delete</param>
        void addReferee(TPerson.Referee referee);
        /// <summary>
        /// remove referee
        /// </summary>
        /// <param name="referee">referee object to delete</param>
        void removeReferee(TPerson.Referee referee);
        /// <summary>
        /// add team to competition
        /// </summary>
        /// <param name="team">team object</param>
        void addTeam(TTeam.ITeam team);
        /// <summary>
        /// remove team from competition
        /// </summary>
        /// <param name="team">team object</param>
        void removeTeam(TTeam.ITeam team);
    }
    /// <summary>
    /// Main class of TournamentManager
    /// 
    /// </summary>
    class Tournament : ITournament
    {
        private string typeOfTournament;
        private string name;
        private TRound.League league;
        private TRound.PlayOff playoff;
        private List<TPerson.Referee> listReferees = new List<TPerson.Referee>();
        private List<TTeam.ITeam> teams = new List<TTeam.ITeam>();


        Tournament()
        {
            throw new TException.TournamentMustBeDefine();
        }

        Tournament(string name)
        {
            TDException("tournament name");
        }

        Tournament(string name, string type)
        {
            if (string.IsNullOrEmpty(name))
                TDException("tournament name");
            if (string.IsNullOrEmpty(type))
                TDException("tournament type");
            this.name = name;
            this.typeOfTournament = type;

        }

        public void addReferee(Referee referee)
        {
            throw new NotImplementedException();
        }

        public void addTeam(ITeam team)
        {
            throw new NotImplementedException();
        }

        public void removeReferee(Referee referee)
        {
            throw new NotImplementedException();
        }

        public void removeTeam(ITeam team)
        {
            throw new NotImplementedException();
        }

        public void setLeague()
        {
            throw new NotImplementedException();
        }

        public void setPlayOff(List<ITeam> teams)
        {
            throw new NotImplementedException();
        }

        private void TDException(string message)
        {
            throw new TException.TournamentDataException(message);
        }
    }

}
