using System.Collections.Generic;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
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