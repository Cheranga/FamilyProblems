using System.Collections.Generic;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Interfaces
{
    public interface ISearchRelationships
    {
        Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen);
    }
}