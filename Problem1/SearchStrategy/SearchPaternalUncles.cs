using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchPaternalUncles : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {   
            var siblings = new SearchSibling().Find(citizen.Father);
            if (siblings.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = siblings.Message
                };
            }

            var uncles = new List<ICitizen>();
            uncles.AddRange(siblings.Data.Where(x=>x.Sex == Sex.Male));
            uncles.AddRange(siblings.Data.Where(x => x.Sex == Sex.Female && x.Partner != null && x.Partner.Sex == Sex.Male).Select(x => x.Partner));
            
            if (uncles.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(uncles)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no paternal uncles"
            };
        }
    }
}