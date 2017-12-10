using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchGrandChildren : ISearchRelationships
    {
        public virtual Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var children = citizen.Children;
            if (children == null || children.Any() == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = "Does not have any grandchildren"
                };
            }

            var grandChildren = children.SelectMany(x => x.Children ?? new List<ICitizen>()).ToList();
            if (grandChildren.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(grandChildren)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = $"[{citizen.Name}] does not have any grandchildren"
            };
        }
    }

    public class SearchGrandSons : SearchGrandChildren
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var grandSons = status.Data.Where(x => x.Sex == Sex.Male).ToList();
            if (grandSons.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(grandSons)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no grand sons"
            };
        }
    }

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
}