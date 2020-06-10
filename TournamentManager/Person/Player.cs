using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TPerson
    {
        public class Player : Person
        {
            private Byte Number { get; set; }

            public Player(string firstName, string lastName, Byte age, Byte number) : base(firstName, lastName, age)
            {
                this.Number = number;
            }
        }
    }
}
