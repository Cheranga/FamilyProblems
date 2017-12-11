using System.Collections.Generic;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public abstract class BaseSearchRelationship : ISearchRelationships
    {
        protected virtual Status<bool> IsValid(ICitizen citizen)
        {
            var status = citizen != null;

            return new Status<bool>
            {
                IsValid = status,
                Message = status ? string.Empty : "Does not exist"
            };
        }

        


        public abstract Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen);
    }
}