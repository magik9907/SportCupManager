using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{
    namespace TPerson
    {
        public class Player : Person
        {
            [JsonProperty]
            public Byte Number { get; set; }

            public Player(string firstName, string lastName, Byte age, Byte number) : base(firstName, lastName, age)
            {
                this.Number = number;
            }
        }
    }
}
