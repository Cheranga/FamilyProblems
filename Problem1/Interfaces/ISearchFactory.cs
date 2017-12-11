using Problem1.Models;

namespace Problem1.Interfaces
{
    public interface ISearchFactory
    {
        Status<ISearchRelationships> GetSearch(string searchCriteria);

        Status<bool> RegisterSearch(string searchCriteria, ISearchRelationships search);
    }
}