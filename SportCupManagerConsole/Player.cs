using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    class Player : Person
    {
        private Byte Number { get; set; }

        public Player(string firstname, string lastname, Byte age, Byte number) : base(firstname, lastname, age)
        {
            this.Number = number;
        }
    }
}
