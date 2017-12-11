using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchSiblings : BaseSearchRelationship
    {
        protected override string NotFoundMessage
        {
            get { return "There are no siblings"; }
        }

        public override string Name
        {
            get { return "Sibling"; }
        }

        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && citizen.Father != null;
            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "There are no siblings"
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

            var siblings = citizen.Father.Children.Where(x => x != citizen).ToList();

            if (siblings.Any() == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = "There are no siblings"
                };
            }

            return GetFilteredResults(new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = new ReadOnlyCollection<ICitizen>(siblings)
            });
        }
    }
}