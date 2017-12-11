using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchCousins : BaseSearchRelationship
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

            var cousins = new List<ICitizen>();

            var fathersSiblings = new SearchSiblings().Find(citizen.Father);
            if (fathersSiblings.IsValid)
            {
                var searchChildren = new SearchChildren();
                fathersSiblings.Data.ToList().ForEach(x =>
                {
                    var children = searchChildren.Find(x);
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