using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using TournamentManager.TPerson;
using TournamentManager.TTeam;

namespace TournamentManagerTest
{
    [TestClass]
    public class TeamTest
    {
        [TestMethod]
        public void TestPlayerFullname()
        {
            Player a = new Player("Jan", "Kowalski", 23, 6);
            Assert.AreEqual(a.Firstname + " " + a.Lastname, a.Fullname);
        }
        
        [TestMethod]
        public void ConstructorTestTeam()
        {
            Assert.ThrowsException<TournamentManager.TException.TeamMissingNameException>(
                () => new DodgeballTeam(null)
            );
            Assert.ThrowsException<TournamentManager.TException.TeamMissingNameException>(
                () => new TugOfWarTeam(null)
            );
            Assert.ThrowsException<TournamentManager.TException.TeamMissingNameException>(
                () => new VolleyballTeam(null)
            );
        }
    }
}