using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchChildren : BaseSearchRelationship
    {
        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && (citizen.Children != null && citizen.Children.Any());
            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "There are no children"
            };
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

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = citizen.Children
            };
        }
    }

    public class SearchSons : SearchChildren
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var sons = status.Data.Where(x => x.Sex == Sex.Male).ToList();
            if (sons.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(sons)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = $"There are no sons"
            };
        }
    }

    public class SearchDaughters : SearchChildren
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var daughters = status.Data.Where(x => x.Sex == Sex.Female).ToList();
            if (daughters.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(daughters)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = $"There are no daughters"
            };
        }
    }
}