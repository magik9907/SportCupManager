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
            [JsonProperty("name")]
            private string name;
            public Referee(string name, string surname, byte age):base(name, surname,age)
            {
                this.name = name;
            }
        }
    }
}
