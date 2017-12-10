using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchParent : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var father = new SearchFather().Find(citizen);
            var mother = new SearchMother().Find(citizen);

            if (father.IsValid == false && mother.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = "There are no parents"
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = new ReadOnlyCollection<ICitizen>(new[]
                {
                    father.Data.First(),
                    mother.Data.First()
                })
            };
        }
    }

    public class SearchFather : SearchParent
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = citizen.Father != null,
                Message = citizen.Father == null ? "There is no father" : string.Empty,
                Data = new ReadOnlyCollection<ICitizen>(new[] { citizen.Father })
            };
        }
    }

    public class SearchMother : SearchParent
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = citizen.Mother != null,
                Message = citizen.Mother == null ? "There is no mother" : string.Empty,
                Data = new ReadOnlyCollection<ICitizen>(new[] { citizen.Mother })
            };
        }
    }
}