using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchMaternalAunts : BaseSearchRelationship
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = IsValid(citizen);
            if (status.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = status.Message
                };
            }

            var siblings = new SearchSiblings().Find(citizen.Mother);
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