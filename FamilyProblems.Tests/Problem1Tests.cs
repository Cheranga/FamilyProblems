using System;
using System.Linq;
using Lengaburu.Business;
using Lengaburu.Core.Models;
using Lengaburu.Core.Search.Factories;
using Lengaburu.Core.Search.Identifier;
using Lengaburu.Core.Search.SearchStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lengaburu.Tests
{
    [TestClass]
    public partial class RegistrarTests
    {
        private Registrar _registrar;

        #region Initialization

        [TestInitialize]
        public void Init()
        {
            //
            // The strategies are defined as strings because in runtime or via configuration we can change, the algorithm name.
            // If we used an enum, whenever we want to add/change a strategy we would need to modify the enum
            //
            // NOTE:
            // Some of this classes requires constructor dependencies, and the solution have been designed considering that DI will
            // be implemented in future [Furthermore some of these searches can be abstracted out for it's own interfaces] so it might be
            // easy to do DI, otherwise we need to do context specific DI
            //
            InitRegistrar();
            //
            // Setup the initial data
            //
            InitData();
        }

        private void InitRegistrar()
        {
            var uniqueSearch = new IdentifierByName();

            var searchFactory = new SearchFactory();
            searchFactory.RegisterSearch("paternaluncles", new SearchPaternalUncles(), -1);
            searchFactory.RegisterSearch("maternaluncles", new SearchMaternalUncles(), -1);
            searchFactory.RegisterSearch("paternalaunts", new SearchPaternalAunts(), -1);
            searchFactory.RegisterSearch("maternalaunts", new SearchMaternalAunts(), -1);
            searchFactory.RegisterSearch("sisterinlaws", new SearchSisterInLaws(), 0);
            searchFactory.RegisterSearch("brotherinlaws", new SearchBrotherInLaws(), 0);
            searchFactory.RegisterSearch("cousins", new SearchCousins(new SearchSiblings(), new SearchChildren()), 0);
            searchFactory.RegisterSearch("father", new SearchFather(), -1);
            searchFactory.RegisterSearch("mother", new SearchMother(), -1);
            searchFactory.RegisterSearch("children", new SearchChildren(), 1);
            searchFactory.RegisterSearch("sons", new SearchSons(), 1);
            searchFactory.RegisterSearch("daughters", new SearchDaughters(), 1);
            searchFactory.RegisterSearch("siblings", new SearchSiblings(), 0);
            searchFactory.RegisterSearch("brothers", new SearchBrothers(), 0);
            searchFactory.RegisterSearch("sisters", new SearchSisters(), 0);
            searchFactory.RegisterSearch("grandchildren", new SearchGrandChildren(), 2);
            searchFactory.RegisterSearch("granddaughters", new SearchGrandDaughters(), 2);
            searchFactory.RegisterSearch("grandsons", new SearchGrandSons(), 2);
            searchFactory.RegisterSearch("thegirlchild", new SearchTheGirlChild(new SearchGrandChildren(), new SearchDaughters()), 2);

            _registrar = new Registrar(uniqueSearch, searchFactory);
        }

        private void InitData()
        {
            var kingShan = new Citizen("King Shan", Sex.Male);
            _registrar.AddCitizen(kingShan);

            _registrar.AddPartner(kingShan, new Citizen("Queen Anga", Sex.Female));

            var ish = new Citizen("Ish", Sex.Male);
            var chit = new Citizen("Chit", Sex.Male);
            var vich = new Citizen("Vich", Sex.Male);
            var satya = new Citizen("Satya", Sex.Female);

            var drita = new Citizen("Drita", Sex.Male);
            var vrita = new Citizen("Vrita", Sex.Male);
            var vila = new Citizen("Vila", Sex.Male);
            var chika = new Citizen("Chika", Sex.Female);
            var satvy = new Citizen("Satvy", Sex.Female);
            var savya = new Citizen("Savya", Sex.Male);
            var sayan = new Citizen("Saayan", Sex.Male);

            var jata = new Citizen("Jata", Sex.Male);
            var driya = new Citizen("Driya", Sex.Female);
            var lavnya = new Citizen("Lavnya", Sex.Female);
            var kriya = new Citizen("Kriya", Sex.Male);
            var misa = new Citizen("Misa", Sex.Male);

            _registrar.AddChild(kingShan, ish);
            _registrar.AddChild(kingShan, chit);
            _registrar.AddChild(kingShan, vich);
            _registrar.AddChild(kingShan, satya);

            _registrar.AddPartner(chit, new Citizen("Ambi", Sex.Female));
            _registrar.AddPartner(vich, new Citizen("Lika", Sex.Female));
            _registrar.AddPartner(satya, new Citizen("Vyan", Sex.Male));

            _registrar.AddChild(chit, drita);
            _registrar.AddChild(chit, vrita);

            _registrar.AddChild(vich, vila);
            _registrar.AddChild(vich, chika);

            _registrar.AddChild(satya, satvy);
            _registrar.AddChild(satya, savya);
            _registrar.AddChild(satya, sayan);

            _registrar.AddPartner(drita, new Citizen("Jaya", Sex.Female));
            _registrar.AddPartner(vila, new Citizen("Jnki", Sex.Female));
            _registrar.AddPartner(chika, new Citizen("Kpila", Sex.Male));
            _registrar.AddPartner(satvy, new Citizen("Asva", Sex.Male));
            _registrar.AddPartner(savya, new Citizen("Krpi", Sex.Female));
            _registrar.AddPartner(sayan, new Citizen("Mina", Sex.Female));

            _registrar.AddChild(drita, jata);
            _registrar.AddChild(drita, driya);

            _registrar.AddChild(vila, lavnya);

            _registrar.AddChild(savya, kriya);

            _registrar.AddChild(sayan, misa);

            _registrar.AddPartner(driya, new Citizen("Minu", Sex.Male));

            _registrar.AddPartner(lavnya, new Citizen("Gru", Sex.Male));
        }

        #endregion

        #region Paternal Uncles

        [TestMethod]
        public void FindPaternalUnclesWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("drita", "paternaluncles");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"ish", "vich", "vyan"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindPaternalUnclesWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("vich", "paternaluncles");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Maternal Uncles

        [TestMethod]
        public void FindMaternalUnclesWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("satvy", "maternaluncles");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"ish", "vich", "chit"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindMaternalUnclesWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("misa", "maternaluncles");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Paternal Aunts

        [TestMethod]
        public void FindPaternalAuntsWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("drita", "paternalaunts");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"lika", "satya"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindPaternalAuntsWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("savya", "paternalaunts");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Maternal Aunts

        [TestMethod]
        public void FindMaternalAuntsWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("drita", "paternalaunts");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"lika", "satya"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindMaternalAuntsWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("jnki", "paternalaunts");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Sister in Laws

        [TestMethod]
        public void FindSisterInLawsWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("jnki", "sisterinlaws");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"chika"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindSisterInLawsWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("chika", "sisterinlaws");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Brother in Laws

        [TestMethod]
        public void FindBrotherInLawsWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("vyan", "brotherinlaws");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"vich", "chit", "ish"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindBrotherInLawsWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("satya", "brotherinlaws");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Cousins

        [TestMethod]
        public void FindCousinsWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("vila", "cousins");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"drita", "vrita", "satvy", "savya", "saayan"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindCousinsWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("mina", "cousins");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Father

        [TestMethod]
        public void FindFatherWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("vich", "father");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"king shan"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindFatherWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("king shan", "father");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Mother

        [TestMethod]
        public void FindMotherWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("savya", "mother");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"satya"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindMotherWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("queen anga", "mother");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Children

        [TestMethod]
        public void FindChildrenWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("lika", "children");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"vila", "chika"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindChildrenWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("kriya", "children");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Sons

        [TestMethod]
        public void FindSonsWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("saayan", "sons");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"misa"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindSonsWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("jnki", "sons");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Daughters

        [TestMethod]
        public void FindDaughtersWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("jaya", "daughters");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"driya"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindDaughtersWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("savya", "daughters");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Brothers

        [TestMethod]
        public void FindBrothersWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("satvy", "brothers");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"savya", "saayan"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindBrothersWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("kriya", "brothers");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Sisters

        [TestMethod]
        public void FindSistersWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("vich", "sisters");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"satya"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindSistersWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("satvy", "sisters");

            Assert.IsFalse(status.IsValid);
        }

        #endregion

        #region Grand Daughters

        [TestMethod]
        public void FindGrandDaughtersWhenTheyExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("lika", "granddaughters");

            Assert.IsTrue(status.IsValid);
            Assert.IsTrue(new[] {"lavnya"}.All(x => status.Data.Any(y => x.Equals(y, StringComparison.OrdinalIgnoreCase))));
        }

        [TestMethod]
        public void FindGrandDaughtersWhenTheyDoNotExist()
        {
            //
            // Act
            //
            var status = _registrar.Find("vyan", "granddaughters");

            Assert.IsFalse(status.IsValid);
        }

        #endregion
    }
}