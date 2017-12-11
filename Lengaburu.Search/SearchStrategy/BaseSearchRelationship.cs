using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        protected virtual string NotFoundMessage
        {
            get { return "There are no specific relationships"; }
        }

        protected virtual Func<ICitizen, bool> Filter { get; }


        public abstract Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen);


        protected Status<IReadOnlyList<ICitizen>> GetFilteredResults(Status<IReadOnlyList<ICitizen>> results)
        {
            if (results.IsValid == false)
            {
                return results;
            }

            if (Filter == null)
            {
                return results;
            }

            var filtered = results.Data.Where(Filter).ToList();
            if (filtered.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(filtered)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = string.IsNullOrEmpty(NotFoundMessage) ? "There are no filtered results" : NotFoundMessage
            };
        }
    }
}