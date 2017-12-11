using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1
{
    public class Registrar
    {
        private readonly IUniqueSearch<string, ICitizen> _uniqueSearchByName;
        private readonly ISearchFactory _factory;
        private List<ICitizen> _citizens;

        public Registrar(IUniqueSearch<string, ICitizen> uniqueSearchByName, ISearchFactory factory)
        {
            _uniqueSearchByName = uniqueSearchByName;
            _factory = factory;
            _citizens = new List<ICitizen>();
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
            var sayan = new Citizen("Saayan", Sex.Male);

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

        public Status AddCitizen(ICitizen citizen)
        {
            _citizens.Add(citizen);
            return new Status
            {
                IsValid = true
            };
        }

        public Status AddRoot(string name, Sex sex)
        {
            var father = new Citizen($"Father of {name}", Sex.Male) {GenerationLevel = -1};
            AddPartner(father, new Citizen($"Mother of {name}", Sex.Female));

            var citizen = new Citizen(name, sex);
            
            return AddCitizen(citizen);
        }

        public Status AddChild(ICitizen parent, ICitizen child)
        {
            Status status = null;
            try
            {
                parent.AddChild(child);
                parent.Partner.AddChild(child);
                status = new Status
                {
                    IsValid = true
                };

                _citizens.Add(child);

                if (parent.Sex == Sex.Male)
                {
                    child.Father = parent;
                    child.Mother = parent.Partner;
                }
                else
                {
                    child.Mother = parent;
                    child.Father = parent.Partner;
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

        public Status AddChild(string parentName, ICitizen child)
        {
            Status status = null;
            try
            {
                var findParent = _uniqueSearchByName.FindAll(_citizens, parentName);
                if (findParent.IsValid == false)
                {
                    return new Status
                    {
                        IsValid = false,
                        Message = $"[{parentName}] does not exist"
                    };
                }

                var parent = findParent.Data;

                parent.AddChild(child);

                if (parent.Partner != null)
                {
                    parent.Partner.AddChild(child);
                }

                if (parent.Sex == Sex.Male)
                {
                    child.Father = parent;
                    child.Mother = parent.Partner;
                }
                else
                {
                    child.Mother = parent;
                    child.Father = parent.Partner;
                }

                _citizens.Add(child);

                status = new Status
                {
                    IsValid = true
                };
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
                partner.AddPartner(citizen);
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

        private Status<IReadOnlyList<ICitizen>> FindPeople(string name, string searchCriteria)
        {
            var strategy = _factory.GetSearch(searchCriteria);
            if (strategy.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = strategy.Message
                };
            }

            var person = _uniqueSearchByName.FindAll(_citizens, name);
            if (person.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = person.Message
                };
            }

            var searchResults = strategy.Data.Find(person.Data);

            return searchResults;
        }

        public Status<IEnumerable<string>> Find(string name, string searchCriteria)
        {
            var searchResults = FindPeople(name, searchCriteria);

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

        public Status<IEnumerable<string>> TheGirlChild(string grandParent)
        {
            var findPeopleStatus = FindPeople(grandParent, "thegirlchild");
            if (findPeopleStatus.IsValid == false)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = findPeopleStatus.Message
                };
            }

            var dictionary = findPeopleStatus.Data.GroupBy(x => x.Name)
                .Select(x => new
                {
                    Name = x.Key,
                    Count = x.Count()
                })
                .GroupBy(x => x.Count)
                .ToDictionary(x => x.Key, x => x.ToList());

            var moms = dictionary[dictionary.Keys.Max()].Select(x=>x.Name);

            return new Status<IEnumerable<string>>
            {
                IsValid = true,
                Data = moms
            };
        }
    }
}