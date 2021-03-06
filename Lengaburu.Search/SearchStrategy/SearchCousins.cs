using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchCousins : BaseSearchRelationship
    {
        private readonly SearchChildren _searchChildren;
        private readonly SearchSiblings _searchSiblings;

        public SearchCousins(SearchSiblings searchSiblings, SearchChildren searchChildren)
        {
            _searchSiblings = searchSiblings;
            _searchChildren = searchChildren;
        }

        public override string Name
        {
            get { return "Cousin"; }
        }

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

            var cousins = new List<ICitizen>();

            var fathersSiblings = _searchSiblings.Find(citizen.Father);
            if (fathersSiblings.IsValid)
            {
                fathersSiblings.Data.ToList().ForEach(x =>
                {
                    var children = _searchChildren.Find(x);
                    if (children.IsValid)
                    {
                        cousins.AddRange(children.Data);
                    }
                });
            }

            var mothersSiblings = new SearchSiblings().Find(citizen.Mother);
            if (mothersSiblings.IsValid)
            {
                var searchChildren = new SearchChildren();
                mothersSiblings.Data.ToList().ForEach(x =>
                {
                    var children = searchChildren.Find(x);
                    if (children.IsValid)
                    {
                        cousins.AddRange(children.Data);
                    }
                });
            }

            if (cousins.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(cousins)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no cousins"
            };
        }
    }
}