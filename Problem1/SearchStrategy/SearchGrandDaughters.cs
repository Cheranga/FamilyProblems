using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.SearchStrategy
{
    public class SearchGrandDaughters : SearchGrandChildren
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var grandDaughters = status.Data.Where(x => x.Sex == Sex.Female).ToList();
            if (grandDaughters.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(grandDaughters)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no grand daughters"
            };
        }
    }

    public class TheGirlChild : BaseSearchRelationship
    {
        private readonly SearchGrandChildren _searchGrandChildren;
        private readonly SearchDaughters _searchDaughters;

        public TheGirlChild(SearchGrandChildren searchGrandChildren, SearchDaughters searchDaughters)
        {
            _searchGrandChildren = searchGrandChildren;
            _searchDaughters = searchDaughters;
        }

        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = _searchGrandChildren.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var moms = new List<ICitizen>();

            status.Data.ToList().ForEach(x =>
            {
                if (x.Sex == Sex.Female)
                {
                    moms.Add(x.Mother);
                }

                var daughters = _searchDaughters.Find(x);
                if (daughters.IsValid)
                {
                    moms.AddRange(daughters.Data.Select(y => y.Mother));
                }
            });

            if (moms.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(moms)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no grand daughters or great grand daughters"
            };
        }
    }
}