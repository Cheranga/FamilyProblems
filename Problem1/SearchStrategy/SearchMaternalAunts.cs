using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchMaternalAunts : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var siblings = new SearchSibling().Find(citizen.Mother);
            if (siblings.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = siblings.Message
                };
            }

            var aunts = new List<ICitizen>();
            aunts.AddRange(siblings.Data.Where(x => x.Sex == Sex.Female));
            aunts.AddRange(siblings.Data.Where(x => x.Sex == Sex.Male && x.Partner != null && x.Partner.Sex == Sex.Female).Select(x => x.Partner));

            if (aunts.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(aunts)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no paternal aunts"
            };
        }
    }
}