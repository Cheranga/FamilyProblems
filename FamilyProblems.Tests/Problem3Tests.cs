using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Problem1.Models;

namespace FamilyProblems.Tests
{   
    public partial class RegistrarTests
    {
        [TestMethod]
        public void GetYourselfSomeDaughters()
        {
            //
            // Act
            //
            var girlChildStatus = _registrar.TheGirlChild("king shan");
            //
            // Assert
            //
            Assert.IsTrue(girlChildStatus.IsValid);
            Assert.IsTrue(new[] { "jaya", "jnki", "satya", "lika"}.All(x => girlChildStatus.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void WhoIsYourFavoriteDaughterNow()
        {
            //
            // Act
            //
            _registrar.AddChild("jaya", new Citizen("drini", Sex.Female));
            var girlChildStatus = _registrar.TheGirlChild("king shan");
            //
            // Assert
            //
            Assert.IsTrue(girlChildStatus.IsValid);
            Assert.IsTrue(new[] { "jaya"}.All(x => girlChildStatus.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void GreatGrandDaughterHavingADaughterDoesNotMeanAThing()
        {
            //
            // Act
            //
            _registrar.AddChild("lavnya", new Citizen("drini", Sex.Female));
            var girlChildStatus = _registrar.TheGirlChild("king shan");
            //
            // Assert
            //
            Assert.IsTrue(girlChildStatus.IsValid);
            Assert.IsTrue(new[] { "jaya", "jnki", "satya", "lika" }.All(x => girlChildStatus.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }
    }
}
