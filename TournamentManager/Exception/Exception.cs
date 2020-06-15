using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TException
    {
        public class TournamentDyscyplineNotIdentify : Exception
        {
            public TournamentDyscyplineNotIdentify() : base("unknown tournament dyscypline")
            {

            }
        }

        public class TournamentNameMustBeDefine : Exception
        {
            public TournamentNameMustBeDefine() : base("Tournament Name is not defined")
            {

            }
        }

        /// <summary>
        /// custom exception when no data given
        /// </summary>
        public class TournamentMustBeDefine : Exception
        {
            public TournamentMustBeDefine() : base("Name and type must be define")
            {

            }
        }
        /// <summary>
        /// custom exception of tournament data
        /// </summary>
        public class TournamentDataException : Exception
        {
            /// <summary>
            /// define what type of incorect data exception
            /// </summary>
            /// <param name="type">type of exception</param>
            public TournamentDataException(string type) : base(type + "must be define")
            {

            }


        }
        /// <summary>
        /// throw exception of not enought teams in tournament
        /// </summary>
        public class NotEnoughtTeamsNumber: Exception{
            
            /// <param name="number">current number of teams</param>
            public NotEnoughtTeamsNumber(int number):base("insufficient number of teams. Number of teams: "+number)
            { 
        
            }
        }
        /// <summary>
        /// exception of object dind't create
        /// </summary>
        public class ObjectNotDefined : Exception
        {
            /// <param name="TObject">type of set type</param>
            public ObjectNotDefined(string TObject) : base(TObject + " is not defined")
            {

            }
        }

        public class TeamMissingNameException : Exception
        {
            public TeamMissingNameException() : base("There is missing name parameter for Team.")
            {

            }
        }
    }
}
