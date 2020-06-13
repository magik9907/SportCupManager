using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TournamentManager
{
    namespace TPerson {
        public abstract class Person : IPerson
        {
            public Byte Age { get; set; }

            public string Firstname { get; set; }

            public string Lastname { get; set; }
            [JsonIgnore]
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
}
