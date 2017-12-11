using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.SearchStrategy
{
    public class SearchPaternalUncles : BaseSearchRelationship
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

            var siblings = new SearchSibling().Find(citizen.Father);
            if (siblings.IsValid == false)
            {
               return siblings;
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