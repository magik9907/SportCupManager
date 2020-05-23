using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    class Player : Person
    {
        private Byte number;

        public Player(string firstName, string lastName, Byte age, Byte number) : base(firstName, lastName, age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
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
