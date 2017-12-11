using System.Collections.Generic;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.SearchStrategy
{
    public class SearchInLawSiblings : BaseSearchRelationship
    {
        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && (citizen.Partner != null);
            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "There are no in laws"
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

            var partnersSiblings = new SearchSibling().Find(citizen.Partner);
            if (partnersSiblings.IsValid)
            {
               return partnersSiblings;
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no in laws"
            };
        }
    }
}