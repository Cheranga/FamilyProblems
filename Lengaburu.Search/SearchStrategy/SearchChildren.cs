using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchChildren : BaseSearchRelationship
    {
        protected override string NotFoundMessage
        {
            get { return "There are no children"; }
        }

        protected override Status<bool> IsValid(ICitizen citizen)
        {
            var status = base.IsValid(citizen).IsValid && (citizen.Children != null && citizen.Children.Any());
            return new Status<bool>
            {
                IsValid = status,
                Message = status? string.Empty :string.IsNullOrEmpty(NotFoundMessage)? "There are no children" : NotFoundMessage
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
                    Message = string.IsNullOrEmpty(NotFoundMessage) ? "There are no children" : NotFoundMessage
                };
            }

            return GetFilteredResults(new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = children
            });
        }
    }
}