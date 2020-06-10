using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace TournamentManagerTest
{
    [TestClass]
    public class TournamentTest
    {
        [TestMethod]
        public void ConstructorTestNoParam()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentMustBeDefine>(
                () => new TournamentManager.Tournament()
            ) ;
        }

        [TestMethod]
        public void ConstructorTestOneParam()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentDataException>(
                () => new TournamentManager.Tournament("test")
            );
        }

        [TestMethod]
        public void ConstructorTestNull_NULL()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentMustBeDefine>(
                () => new TournamentManager.Tournament(null, 0)
            ); ;
        }
        [TestMethod]
        public void ConstructorTestString_null()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentDataException>(
                () => new TournamentManager.Tournament("test",0)
            );
        }

        [TestMethod]
        public void ConstructorTestOneNull_String()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentDataException>(
                () => new TournamentManager.Tournament(null, (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball)
            );
        }

        [TestMethod]
        public void ConstructorTestDataVerify()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);

            Assert.AreEqual("name",t.Name);
            Assert.IsTrue(Enum.IsDefined(typeof(TournamentManager.TEnum.TournamentDyscypline),t.Dyscypline));

        }

        [TestMethod]
        public void TestRemoveRefereeNoDefined()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);
            TournamentManager.TPerson.Referee refA = (TournamentManager.TPerson.Referee)CreatePerson("Referee","imei","nazwisko",5);
            try
            {
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee","imei", "nazwisko", 5));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.AddReferee(refA);
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));

                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.RemoveReferee(refA);
                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsFalse(t.Referees.Contains(refA));

        }

        [TestMethod]
        public void TestAddReferee()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);
            TournamentManager.TPerson.Referee refA = (TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5);
            try
            {
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.AddReferee(refA);
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee", "imei", "nazwisko", 5));
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsTrue(t.Referees.Contains(refA));

        }

        [TestMethod]
        public void TestAddRefereeNoDefined()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);
            TournamentManager.TPerson.Referee refA =(TournamentManager.TPerson.Referee) CreatePerson("Referee", "imei", "nazwisko", 5);

            Assert.ThrowsException<TournamentManager.TException.ObjectNotDefined>(
                () =>
                {
                    t.AddReferee(null);
                });
        }

        [TestMethod]
        public void TestRemoveTeam()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);
            TournamentManager.TTeam.ITeam teamA = CreateTeam("DodgeBall","name");
            try
            {
                t.AddTeam(CreateTeam("DodgeBall", "name"));
                t.AddTeam(CreateTeam("DodgeBall", "name"));
                t.AddTeam(CreateTeam("DodgeBall", "name"));
                t.AddTeam(CreateTeam("DodgeBall", "name"));

                t.RemoveTeam(teamA);

            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsFalse(t.Teams.Contains(teamA));

        }

        [TestMethod]
        public void TestAddTeam()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);
            TournamentManager.TTeam.ITeam teamA = CreateTeam("DodgeBall", "name");
            try
            {
                t.AddTeam(CreateTeam("DodgeBall", "name"));
                t.AddTeam(CreateTeam("DodgeBall", "name"));
                t.AddTeam(teamA);
                t.AddTeam(CreateTeam("DodgeBall", "name"));
                t.AddTeam(CreateTeam("DodgeBall", "name"));
            }
            catch (System.Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsTrue(t.Teams.Contains(teamA));

        }

        [TestMethod]
        public void TestAddTeamNoDefined()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", (int)TournamentManager.TEnum.TournamentDyscypline.dodgeball);
            TournamentManager.TTeam.ITeam teamA = CreateTeam("DodgeBall", "name");

            Assert.ThrowsException<TournamentManager.TException.ObjectNotDefined>(
                () =>
                {
                    t.AddTeam(null);
                });
        }

        private TournamentManager.TTeam.ITeam CreateTeam(string type,string name)
        {
            switch (type)
            {
                case "DodgeBall":
                    return (TournamentManager.TTeam.ITeam)new TournamentManager.TTeam.DodgeballTeam(name);
                case "TugOfWar":
                    return (TournamentManager.TTeam.ITeam)new TournamentManager.TTeam.TugOfWarTeam(name);
                case "Voleyball":
                    return (TournamentManager.TTeam.ITeam)new TournamentManager.TTeam.VolleyballTeam(name);
                default: return null;
            }
        }

        private TournamentManager.TPerson.IPerson CreatePerson(string type, string name, string surname,byte age)
        {
            return CreatePerson( type, name, surname, age, 0);
        }

        private TournamentManager.TPerson.IPerson CreatePerson(string type,string name, string surname, byte age, byte number)
        {
            switch (type)
            {
                case "Referee":
                    return (TournamentManager.TPerson.IPerson)new TournamentManager.TPerson.Referee(name, surname, age);
                case "Player":
                    return (TournamentManager.TPerson.IPerson)new TournamentManager.TPerson.Player(name, surname, age, number);
                default:return null;
            }
        }
    }
}
