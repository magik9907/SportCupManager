using System;
using System.Collections.Generic;
using System.Text;

namespace SportCupManagerConsole
{
    public abstract class Person : IPerson
    {
        public Byte Age { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }        

        public Person(string firstname, string lastname, Byte age)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Age = age;
        }
    }
}
