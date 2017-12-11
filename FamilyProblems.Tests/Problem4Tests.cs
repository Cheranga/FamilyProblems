using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lengaburu.Tests
{
    public partial class RegistrarTests
    {
        [TestMethod]
        public void FindingPaternalUnclesWhenExists()
        {
            //
            // Act
            //
            var status = _registrar.WhoAreYou("kriya", "saayan");
            //
            // Assert
            //
            Assert.IsTrue(status.IsValid);
            Assert.AreEqual("Paternal Uncle", string.Join("", status.Data));
        }

        [TestMethod]
        public void FindGrandSonWhenExists()
        {
            //
            // Act
            //
            var status = _registrar.WhoAreYou("satya", "kriya");
            //
            // Assert
            //
            Assert.IsTrue(status.IsValid);
            Assert.AreEqual("Grand Child, Grand Son", string.Join(", ", status.Data));
        }

        [TestMethod]
        public void FindingTheGirlChildWithJustProvidingNames()
        {
            //
            // Act
            //
            var status = _registrar.WhoAreYou("king shan", "jaya");
            //
            // Assert
            //
            Assert.IsTrue(status.IsValid);
            Assert.AreEqual("The Girl Child", string.Join("", status.Data));
        }
    }
}