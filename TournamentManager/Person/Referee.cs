using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{
    namespace TPerson
    {
        public interface IRefereeId
        {
            int Id
            {
                get;
            }
        }

        public class Referee : Person, IRefereeId
        {
            [JsonProperty("name")]
            private string name;
            private int idReferee;
            public int Id { get { return idReferee; } }
            public Referee(string name, string surname, byte age, int id):base(name, surname,age)
            {
                this.name = name;
                idReferee = id;
            }

        }
    }
}
