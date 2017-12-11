using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchGrandChildren : BaseSearchRelationship
    {
        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && (citizen.Children != null && citizen.Children.Any());
            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "There are no grandchildren"
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

            var children = citizen.Children;
            if (children == null || children.Any() == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = "There are no grandchildren"
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
                Message = "There are no grandchildren"
            };
        }
    }
}