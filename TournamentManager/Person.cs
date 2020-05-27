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

        public void setFirstName(string name)
        {
            this.firstName = name;
        }

        public string getFirstName()
        {
            return firstName;
        }

        public void setLastName(string name)
        {
            this.lastName = name;
        }

        public string getLastName()
        {
            return lastName;
        }

        public void setAge(Byte age)
        {
            this.age = age;
        }

        public Byte getAge()
        {
            return age;
        }
    }
}
