using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchCousins : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {   
            var cousins = new List<ICitizen>();

            var fathersSiblings = new SearchSibling().Find(citizen.Father);
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

            var mothersSiblings = new SearchSibling().Find(citizen.Mother);
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