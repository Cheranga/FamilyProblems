using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchParent : BaseSearchRelationship
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
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

            if (father.IsValid)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = father.Data
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = mother.Data
            };
        }
    }

    public class SearchFather : BaseSearchRelationship
    {
        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && citizen.Father != null;
            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "There is no father"
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
                Data = new ReadOnlyCollection<ICitizen>(new[] { citizen.Father })
            };
        }
    }

    public class SearchMother : BaseSearchRelationship
    {
        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && citizen.Mother != null;
            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "There is no mother"
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
                Data = new ReadOnlyCollection<ICitizen>(new[] { citizen.Mother })
            };
        }
    }
}