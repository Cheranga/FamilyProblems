using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchGrandChildren : BaseSearchRelationship
    {
        protected override string NotFoundMessage
        {
            get { return "There are no grand children"; }
        }

        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && (citizen.Children != null && citizen.Children.Any());
            return new Status<bool>
            {
                IsValid = status,
                Message = string.IsNullOrEmpty(NotFoundMessage) ? "There are no grand children" : NotFoundMessage
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

            var grandChildren = children.SelectMany(x => x.Children ?? new List<ICitizen>()).ToList();
            if (grandChildren.Any() == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = string.IsNullOrEmpty(NotFoundMessage) ? "There are no grand children" : NotFoundMessage
                };
            }

            return GetFilteredResults(new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = new ReadOnlyCollection<ICitizen>(grandChildren)
            });
        }
    }
}