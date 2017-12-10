using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Problem1;
using Problem1.Interfaces;
using Problem1.Models;
using Problem1.SearchStrategy;

namespace FamilyProblems.Tests
{
    [TestClass]
    public class RegistrarTests
    {
        //private ICitizen _root;

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //    var shanFamily = new Citizen("King Shan", Sex.Male).AddPartner("Queen Anga", Sex.Female);

        //    var chitFamily = new Citizen("Chit", Sex.Male).AddPartner("Ambi", Sex.Female);
        //    var vichFamily = new Citizen("Vich", Sex.Male).AddPartner("Lika", Sex.Female);
        //    var satyaFamily = new Citizen("Satya", Sex.Female).AddPartner("Vyan", Sex.Male);

        //    var dritaFamily = new Citizen("Drita", Sex.Male).AddPartner("Jaya", Sex.Female);
        //    var vilaFamily = new Citizen("Vila", Sex.Male).AddPartner("Jnki", Sex.Female);
        //    var chikaFamily = new Citizen("Chika", Sex.Female).AddPartner("Kpila", Sex.Male);
        //    var satvyFamily = new Citizen("Satvy", Sex.Female).AddPartner("Asva", Sex.Male);
        //    var savyaFamily = new Citizen("Savya", Sex.Male).AddPartner("Krpi", Sex.Female);
        //    var sayanFamily = new Citizen("Saayan", Sex.Male).AddPartner("Mina", Sex.Female);

        //    var driyaFamily = new Citizen("Driya", Sex.Female).AddPartner("Mnu", Sex.Male);
        //    var lavnyaFamily = new Citizen("Lavnya", Sex.Female).AddPartner("Gru", Sex.Male);

        //    //
        //    // Adding children
        //    //
        //    shanFamily.AddChild("Ish", Sex.Male).AddChild(chitFamily).AddChild(vichFamily).AddChild(satyaFamily);
        //    chitFamily.AddChild(dritaFamily).AddChild("Vrita", Sex.Male);
        //    vichFamily.AddChild(vilaFamily).AddChild(chikaFamily);
        //    satyaFamily.AddChild(satvyFamily).AddChild(savyaFamily).AddChild(sayanFamily);
        //    dritaFamily.AddChild("Jata", Sex.Male).AddChild(driyaFamily);
        //    vilaFamily.AddChild(lavnyaFamily);
        //    savyaFamily.AddChild("Kriya", Sex.Male);
        //    sayanFamily.AddChild("Misa", Sex.Male);

        //    _root = shanFamily;
        //}

        [TestMethod]
        public void FindPeopleTest()
        {
            //
            // Arrange
            //
            var registrar = new Registrar(new SearchByName(), new SearchFactory(new Dictionary<string, ISearchRelationships>()
            {
                {"paternaluncles", new SearchPaternalUncles() }
            }),null);

            var status = registrar.FindPeople("drita", "paternaluncles");


            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(status.Data.Count() == 2);
            Assert.IsTrue(status.Data.Any(x=>string.Equals("chit", x, StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(status.Data.Any(x => string.Equals("vich", x, StringComparison.OrdinalIgnoreCase)));
        }
    }
}