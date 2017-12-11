using System.Collections.Generic;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Interfaces
{
    public interface ISearchFactory
    {
        Status<ISearchRelationships> GetSearch(string searchCriteria);

        Status<IEnumerable<ISearchRelationships>> GetSearchFor(int myLevel, int yourLevel);

        Status<bool> RegisterSearch(string searchCriteria, ISearchRelationships search, int searchLevel);
    }
}