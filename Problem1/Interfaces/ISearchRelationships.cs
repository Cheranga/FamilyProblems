using System.Collections.Generic;
using Problem1.Models;

namespace Problem1.Interfaces
{
    public interface ISearchRelationships
    {
        Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen);
    }
}