using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchUncleByParent : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen, Sex sex)
        {
            var parent = sex == Sex.Male ? citizen.Father : citizen.Mother;

            var uncles = new List<ICitizen>();

            var parentsBrothers = new SearchSibling().Find(parent, Sex.Male);
            if (parentsBrothers.IsValid)
            {
                uncles.AddRange(parentsBrothers.Data);
            }

            var parentsSisters = new SearchSibling().Find(parent, Sex.Female);
            if (parentsSisters.IsValid)
            {
                var partners = parentsSisters.Data.Where(x => x.Partner != null && x.Partner.Sex == Sex.Male);
                uncles.AddRange(partners);
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = new ReadOnlyCollection<ICitizen>(uncles)
            };
        }
    }
}