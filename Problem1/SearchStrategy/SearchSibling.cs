using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchSibling : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var siblings = citizen.Father.Children.Where(x => x != citizen).ToList();

            if (siblings.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(siblings)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = $"[{citizen.Name}] is the only child"
            };
        }
    }

    public class SearchBrothers : SearchSibling
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var brothers = status.Data.Where(x => x.Sex == Sex.Male).ToList();
            if (brothers.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(brothers)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no brothers"
            };
        }
    }

    public class SearchSisters : SearchSibling
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var sisters = status.Data.Where(x => x.Sex == Sex.Female).ToList();
            if (sisters.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(sisters)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no sisters"
            };
        }
    }
}