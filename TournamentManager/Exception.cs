using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TException
    {
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
            public TournamentDataException(string type) : base( type + "must be define")
            {
         
            }

          
        }

    }
}
