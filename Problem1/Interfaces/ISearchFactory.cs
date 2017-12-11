using Lengaburu.Core.Models;

namespace Lengaburu.Core.Interfaces
{
    public interface ISearchFactory
    {
        Status<ISearchRelationships> GetSearch(string searchCriteria);

        Status<bool> RegisterSearch(string searchCriteria, ISearchRelationships search);
    }
}