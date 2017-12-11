using System;
using System.Collections.Generic;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Business
{
    public class Registrar
    {
        private readonly List<ICitizen> _citizens;
        private readonly ISearchFactory _factory;
        private readonly IUniqueIdentifier<string, ICitizen> _identitySearch;

        public Registrar(IUniqueIdentifier<string, ICitizen> identitySearch, ISearchFactory factory)
        {
            _identitySearch = identitySearch;
            _factory = factory;
            _citizens = new List<ICitizen>();
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
            Status status;
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
            Status status;
            try
            {
                var findParent = _identitySearch.FindAll(_citizens, parentName);
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
            Status status;
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

            var person = _identitySearch.FindAll(_citizens, name);
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

            var moms = dictionary[dictionary.Keys.Max()].Select(x => x.Name);

            return new Status<IEnumerable<string>>
            {
                IsValid = true,
                Data = moms
            };
        }

        public Status<IEnumerable<string>> WhoAreYou(string myName, string yourName)
        {
            if (string.IsNullOrEmpty(myName) || string.IsNullOrEmpty(yourName))
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = "Both names are important"
                };
            }

            var me = _identitySearch.FindAll(_citizens, myName);
            if (me.IsValid == false)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = me.Message
                };
            }

            var you = _identitySearch.FindAll(_citizens, yourName);
            if (you.IsValid == false)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = you.Message
                };
            }

            var myGenLevel = me.Data.GenerationLevel;
            var yourGenLevel = you.Data.GenerationLevel;

            var searches = _factory.GetSearchFor(myGenLevel, yourGenLevel);
            if (searches.IsValid == false)
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = false,
                    Message = "Cannot find a relationship between you"
                };
            }

            var relationships = new List<string>();
            searches.Data.ToList().ForEach(x =>
            {
                var status = x.Find(me.Data);
                if (status.IsValid)
                {
                    if (status.Data.Any(y => y == you.Data))
                    {
                        relationships.Add(x.Name);
                    }
                }
            });

            if (relationships.Any())
            {
                return new Status<IEnumerable<string>>
                {
                    IsValid = true,
                    Data = relationships
                };
            }

            return new Status<IEnumerable<string>>
            {
                IsValid = false,
                Message = "Cannot find a relationship between you"
            };
        }
    }
}