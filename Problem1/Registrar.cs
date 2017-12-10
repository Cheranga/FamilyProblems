using System;
using System.Collections.Generic;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1
{
    public interface IRegistrar
    {
        //Status AddChild(ICitizen parent, ICitizen child);
        //Status AddPartner(ICitizen citizen, ICitizen partner);

        //Status<IEnumerable<string>> FindPeople(string name, string relationship);
    }

    public class SearchFactory
    {
        private readonly IDictionary<string, ISearchRelationships> _searchStrategies;

        public SearchFactory(IDictionary<string, ISearchRelationships> searchStrategies)
        {
            _searchStrategies = searchStrategies;
        }

        public Status<ISearchRelationships> GetSearch(string searchCriteria)
        {
            if (_searchStrategies.ContainsKey(searchCriteria))
            {
                return new Status<ISearchRelationships>
                {
                    IsValid = true,
                    Data = _searchStrategies[searchCriteria]
                };
            }

            return new Status<ISearchRelationships>
            {
                IsValid = false,
                Message = $"There is no search strategy defined for [{searchCriteria}]"
            };
        }
    }

    public class Registrar : IRegistrar
    {
        private readonly ISearch<string, ICitizen> _searchByName;
        private readonly SearchFactory _factory;
        private List<ICitizen> _citizens;

        public Registrar(ISearch<string, ICitizen> searchByName, SearchFactory factory, List<ICitizen> citizens = null)
        {
            _searchByName = searchByName;
            _factory = factory;
            if (citizens == null)
            {
                InitCitizens();
            }
            else
            {
                _citizens = citizens;
            }
        }

        private void InitCitizens()
        {
            _citizens = new List<ICitizen>();

            var kingShan = new Citizen("King Shan", Sex.Male);
            _citizens.Add(kingShan);

            AddPartner(kingShan, new Citizen("Queen Anga", Sex.Female));

            var ish = new Citizen("Ish",Sex.Male);
            var chit = new Citizen("Chit", Sex.Male);
            var vich = new Citizen("Vich", Sex.Male);
            var satya = new Citizen("Satya", Sex.Female);

            var drita = new Citizen("Drita", Sex.Male);
            var vrita = new Citizen("Vrita", Sex.Male);
            var vila = new Citizen("Vila", Sex.Male);
            var chika = new Citizen("Chika", Sex.Female);
            var satvy = new Citizen("Satvy", Sex.Female);
            var savya = new Citizen("Savya", Sex.Male);
            var sayan = new Citizen("Sayan", Sex.Male);

            var jata = new Citizen("Jata", Sex.Male);
            var driya = new Citizen("Driya", Sex.Female);
            var lavnya = new Citizen("Lavnya", Sex.Female);
            var kriya = new Citizen("Kriya", Sex.Male);
            var misa = new Citizen("Misa", Sex.Male);


            AddPartner(chit, new Citizen("Ambi", Sex.Female));
            AddPartner(vich, new Citizen("Lika", Sex.Female));
            AddPartner(satya, new Citizen("Vyan", Sex.Male));

            AddPartner(drita, new Citizen("Jaya", Sex.Female));
            AddPartner(vila, new Citizen("Jnki", Sex.Female));
            AddPartner(chika, new Citizen("Kpila", Sex.Male));
            AddPartner(satvy, new Citizen("Asva", Sex.Male));
            AddPartner(savya, new Citizen("Krpi", Sex.Female));
            AddPartner(sayan, new Citizen("Mina", Sex.Female));

            AddPartner(driya, new Citizen("Minu", Sex.Male));
            AddPartner(lavnya, new Citizen("Gru", Sex.Male));

            AddChild(kingShan, ish);
            AddChild(kingShan, chit);
            AddChild(kingShan, vich);
            AddChild(kingShan, satya);

            AddChild(chit, drita);
            AddChild(chit, vrita);

            AddChild(vich, vila);
            AddChild(vich, chika);

            AddChild(satya, satvy);
            AddChild(satya, savya);
            AddChild(satya, sayan);

            AddChild(drita, jata);
            AddChild(drita, driya);

            AddChild(vila, lavnya);

            AddChild(savya, kriya);

            AddChild(sayan, misa);

        }

        public Status AddChild(ICitizen parent, ICitizen child)
        {
            Status status = null;
            try
            {
                parent.AddChild(child);
                status = new Status
                {
                    IsValid = true
                };

                _citizens.Add(child);

                if (parent.Sex == Sex.Male)
                {
                    child.Father = parent;
                }
                else
                {
                    child.Mother = parent;
                }

            }
            catch (Exception exception)
            {
                status = new Status
                {
                    IsValid = false,
                    Message = exception.Message
                };
            }

            return status;
        }

        public Status AddPartner(ICitizen citizen, ICitizen partner)
        {
            Status status = null;
            try
            {
                citizen.AddPartner(partner);
                status = new Status
                {
                    IsValid = true
                };

                _citizens.Add(partner);
            }
            catch(Exception exception)
            {
                status = new Status
                {
                    IsValid = false,
                    Message = exception.Message
                };
            }

            return status;
        }

        public Status<IEnumerable<string>> FindPeople(string name, string searchCriteria)
        {
            var strategy = _factory.GetSearch(searchCriteria);
            if (strategy.IsValid == false)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = strategy.Message
                };
            }

            var status = _searchByName.FindAll(_citizens, name);
            if (status.IsValid == false)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = status.Message
                };
            }

            var searchResults = strategy.Data.Find(status.Data);

            if (searchResults.IsValid)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = true,
                    Data = searchResults.Data.Select(x => x.Name)
                };
            }


            return new Status<IEnumerable<string>>
            {
                IsValid = false,
                Message = searchResults.Message
            };
        }
    }
}