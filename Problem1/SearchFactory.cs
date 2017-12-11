using System.Collections.Generic;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core
{
    public class SearchFactory : ISearchFactory
    {
        private readonly IDictionary<string, ISearchRelationships> _searchStrategies;

        public SearchFactory(IDictionary<string, ISearchRelationships> searchStrategies)
        {
            _searchStrategies = searchStrategies;
        }

        public Status<ISearchRelationships> GetSearch(string searchCriteria)
        {
            if (_searchStrategies.ContainsKey(searchCriteria))
            {
                return new Status<ISearchRelationships>
                {
                    IsValid = true,
                    Data = _searchStrategies[searchCriteria]
                };
            }

            return new Status<ISearchRelationships>
            {
                IsValid = false,
                Message = $"There is no search strategy defined for [{searchCriteria}]"
            };
        }

        public Status<bool> RegisterSearch(string searchCriteria, ISearchRelationships search)
        {
            if (string.IsNullOrEmpty(searchCriteria) || search == null)
            {
                return new Status<bool>
                {
                    IsValid = false,
                    Message = "Both search criteria and search algorithm are required1"
                };
            }

            ISearchRelationships searchRelationship;
            if (_searchStrategies.TryGetValue(searchCriteria, out searchRelationship))
            {
                return new Status<bool>
                {
                    IsValid = false,
                    Message = $"[{searchCriteria}] already exists"
                };
            }

            _searchStrategies.Add(searchCriteria, search);

            return new Status<bool>
            {
                IsValid = true
            };
        }
    }
}