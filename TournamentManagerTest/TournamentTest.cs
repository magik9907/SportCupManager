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
            Assert.AreEqual("type", t.Type);

        }

    }
}
