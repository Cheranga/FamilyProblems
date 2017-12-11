using System.Collections.Generic;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.Factories
{
    public class SearchFactory : ISearchFactory
    {
        private readonly IDictionary<string, ISearchRelationships> _searchStrategies;
        private readonly IDictionary<int, IDictionary<string, ISearchRelationships>> _searchStrategiesByLevel;


        public SearchFactory()
        {
            _searchStrategies = new Dictionary<string, ISearchRelationships>();
            _searchStrategiesByLevel = new Dictionary<int, IDictionary<string, ISearchRelationships>>();
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

        public Status<IEnumerable<ISearchRelationships>> GetSearchFor(int myLevel, int yourLevel)
        {
            //
            // Negative value denotes that the person we are looking for is older
            //
            var level = (myLevel - yourLevel)*-1;

            if (_searchStrategiesByLevel.ContainsKey(level))
            {
                return new Status<IEnumerable<ISearchRelationships>>
                {
                    IsValid = true,
                    Data = new List<ISearchRelationships>(_searchStrategiesByLevel[level].Values)
                };
            }

            return new Status<IEnumerable<ISearchRelationships>>
            {
                IsValid = false,
                Message = "There are no search criterias defined for this level"
            };
        }

        public Status<bool> RegisterSearch(string searchCriteria, ISearchRelationships search, int searchLevel)
        {
            if (string.IsNullOrEmpty(searchCriteria) || search == null)
            {
                return new Status<bool>
                {
                    IsValid = false,
                    Message = "Both search criteria and search algorithm are required1"
                };
            }
            //
            // TODO: Return status as invalid, if the same is registered
            //
            ISearchRelationships searchRelationship;
            if (_searchStrategies.TryGetValue(searchCriteria, out searchRelationship) == false)
            {
                _searchStrategies.Add(searchCriteria, search);
            }

            IDictionary<string, ISearchRelationships> searches;
            if (_searchStrategiesByLevel.TryGetValue(searchLevel, out searches))
            {
                if (searches.ContainsKey(searchCriteria) == false)
                {
                    searches.Add(searchCriteria, search);
                }
            }
            else
            {
                _searchStrategiesByLevel.Add(searchLevel, new Dictionary<string, ISearchRelationships>
                {
                    {searchCriteria, search}
                });
            }


            return new Status<bool>
            {
                IsValid = true
            };
        }
    }
}