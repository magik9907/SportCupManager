using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    abstract class Person
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public Byte Age { get; set; }

        public Person(string firstname, string lastname, Byte age)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Age = age;
        }
    }
}
