using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchInLawSiblings : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {   
            if (citizen.Partner == null)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = "There are no in laws"
                };
            }

            var partnersSiblings = new SearchSibling().Find(citizen.Partner);
            if (partnersSiblings.IsValid)
            {
               return partnersSiblings;
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no in laws"
            };
        }
    }
}