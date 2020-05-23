using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    abstract class Person
    {
        protected string firstName;
        protected string lastName;
        protected Byte age;

        public Person(string firstName, string lastName, Byte age)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
        }
    }
}
