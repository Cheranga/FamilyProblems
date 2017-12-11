using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchMother : BaseSearchRelationship
    {
        public override string Name
        {
            get { return "Mother"; }
        }

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
                Data = new ReadOnlyCollection<ICitizen>(new[] {citizen.Mother})
            };
        }
    }
}