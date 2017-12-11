using System.Collections.Generic;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Interfaces
{
    public interface IUniqueIdentifier<TSearch, TSearchResult>
    {
        //Status<TSearchResult> Find(IEnumerable<ICitizen> citizens, TSearch search);
        Status<TSearchResult> FindAll(IEnumerable<ICitizen> citizens, TSearch search);
    }
}