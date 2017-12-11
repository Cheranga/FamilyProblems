using System.Collections.Generic;
using Problem1.Models;

namespace Problem1.Interfaces
{
    public interface IUniqueSearch<TSearch, TSearchResult>
    {
        //Status<TSearchResult> Find(IEnumerable<ICitizen> citizens, TSearch search);
        Status<TSearchResult> FindAll(IEnumerable<ICitizen> citizens, TSearch search);
    }
}