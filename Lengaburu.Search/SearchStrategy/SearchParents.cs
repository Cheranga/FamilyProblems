using System.Collections.Generic;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchParents : BaseSearchRelationship
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var father = new SearchFather().Find(citizen);
            var mother = new SearchMother().Find(citizen);

            if (father.IsValid == false && mother.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = "There are no parents"
                };
            }

            if (father.IsValid)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = father.Data
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = mother.Data
            };
        }
    }
}