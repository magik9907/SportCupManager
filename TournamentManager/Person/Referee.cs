using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{
    namespace TPerson
    {
        

        public class Referee : Person
        {
            private int idReferee;
            public int Id { get { return idReferee; } }
            public Referee(string name, string surname, byte age, int id):base(name, surname,age)
            {
                idReferee = id;
            }

        }
    }
}
