using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                () => new TournamentManager.Tournament(null,null)
            );
        }
        [TestMethod]
        public void ConstructorTestString_null()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentDataException>(
                () => new TournamentManager.Tournament("test",null)
            );
        }

        [TestMethod]
        public void ConstructorTestOneNull_String()
        {
            Assert.ThrowsException<TournamentManager.TException.TournamentDataException>(
                () => new TournamentManager.Tournament(null,"test")
            );
        }

        [TestMethod]
        public void ConstructorTestDataVerify()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "type");

            Assert.AreEqual("name",t.Name);
            Assert.AreEqual("type", t.Dyscypline);

        }

        [TestMethod]
        public void TestRemoveRefereeNoDefined()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "Test");
            TournamentManager.TPerson.Referee refA = (TournamentManager.TPerson.Referee)CreatePerson("Referee");
            try
            {
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.AddReferee(refA);
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));

                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.RemoveReferee(refA);
                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.RemoveReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                
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
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "Test");
            TournamentManager.TPerson.Referee refA = (TournamentManager.TPerson.Referee)CreatePerson("Referee");
            try
            {
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.AddReferee(refA);
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
                t.AddReferee((TournamentManager.TPerson.Referee)CreatePerson("Referee"));
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
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "Test");
            TournamentManager.TPerson.Referee refA =(TournamentManager.TPerson.Referee) CreatePerson("Referee");

            Assert.ThrowsException<TournamentManager.TException.ObjectNotDefined>(
                () =>
                {
                    t.AddReferee(null);
                });
        }

        [TestMethod]
        public void TestRemoveTeam()
        {
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "Test");
            TournamentManager.TTeam.ITeam teamA = CreateTeam("DodgeBall");
            try
            {
                t.AddTeam((TournamentManager.TTeam.ITeam)CreateTeam("DodgeBall"));
                t.AddTeam((TournamentManager.TTeam.ITeam)CreateTeam("DodgeBall"));
                t.AddTeam((TournamentManager.TTeam.ITeam)CreateTeam("DodgeBall"));
                t.AddTeam((TournamentManager.TTeam.ITeam)CreateTeam("DodgeBall"));

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
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "Test");
            TournamentManager.TTeam.ITeam teamA = CreateTeam("DodgeBall");
            try
            {
                t.AddTeam(CreateTeam("DodgeBall"));
                t.AddTeam(CreateTeam("DodgeBall"));
                t.AddTeam(teamA);
                t.AddTeam(CreateTeam("DodgeBall"));
                t.AddTeam(CreateTeam("DodgeBall"));
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
            TournamentManager.Tournament t = new TournamentManager.Tournament("name", "Test");
            TournamentManager.TTeam.ITeam teamA = (TournamentManager.TTeam.ITeam)CreateTeam("DodgeBall");

            Assert.ThrowsException<TournamentManager.TException.ObjectNotDefined>(
                () =>
                {
                    t.AddTeam(null);
                });
        }

        private TournamentManager.TTeam.ITeam CreateTeam(string type)
        {
            switch (type)
            {
                case "DodgeBall":
                    return new TournamentManager.TTeam.DodgeballTeam("Dodgeball team");
                case "TugOfWar":
                    return new TournamentManager.TTeam.TugOfWarTeam("Tug of war team");
                case "Voleyball":
                    return new TournamentManager.TTeam.VolleyballTeam("Volleyball team");
                default: return null;
            }
        }

        private TournamentManager.TPerson.IPerson CreatePerson(string type)
        {
            switch (type)
            {
                case "Referee":
                    return new TournamentManager.TPerson.Referee("name", "surname", 20);
                case "Player":
                    return new TournamentManager.TPerson.Player("name2", "surname2", 30, 8);
                default:return null;
            }
        }
    }
}
