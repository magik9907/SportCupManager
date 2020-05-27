using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentManager
{
    namespace TPerson
    {
        public class Player : Person
        {
            private Byte number;

            public Player(string firstName, string lastName, Byte age, Byte number) : base(firstName, lastName, age)
            {
                this.number = number;
            }

            public void setNumber(Byte number)
            {
                this.number = number;
            }

            public Byte getNumber()
            {
                return number;
            }
        }
    }
}
