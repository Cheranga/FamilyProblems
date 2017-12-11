using System;
using System.Linq;
using Lengaburu.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lengaburu.Tests
{
    public partial class RegistrarTests
    {
        [TestMethod]
        public void AddingNewBornToCouple()
        {
            //
            // Act
            //
            var addChildStatus = _registrar.AddChild("lavnya", new Citizen("Vanya", Sex.Female));
            var grandChildrenStatus = _registrar.Find("jnki", "grandchildren");
            //
            // Assert
            //
            Assert.IsTrue(addChildStatus.IsValid);
            Assert.IsTrue(grandChildrenStatus.IsValid);
            Assert.IsTrue(new[] { "vanya"}.All(x => grandChildrenStatus.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void AddingNewBornToNonExistingPerson()
        {
            //
            // Act
            //
            var addChildStatus = _registrar.AddChild("ABC", new Citizen("Vanya", Sex.Female));
            var grandChildrenStatus = _registrar.Find("ABC", "grandchildren");
            //
            // Assert
            //
            Assert.IsFalse(addChildStatus.IsValid);
            Assert.IsFalse(grandChildrenStatus.IsValid);
        }
    }
}
