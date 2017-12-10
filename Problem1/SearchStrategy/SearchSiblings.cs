using System.Collections.Generic;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchSiblings : ISearch<string, ICitizen>
    {
        private readonly SearchByName _searchByName;

        public SearchSiblings(SearchByName searchByName)
        {
            _searchByName = searchByName;
        }

        public Status<IReadOnlyList<ICitizen>> FindAll(IEnumerable<ICitizen> citizens, string search)
        {
            //
            // TODO: Create a base class and perform common validations there
            //
            var findStatus = _searchByName.FindAll(citizens, search);
            if (findStatus.IsValid == false)
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = false,
                    Message = findStatus.Message
                };
            }

            var citizen = findStatus.Data.First();
            var siblings = new List<ICitizen>();
            
            var kidsOfFather = citizen.Father.Children ?? new List<ICitizen>();

            if (kidsOfFather.Any())
            {
                siblings.AddRange(kidsOfFather.Where(x => x.Name != citizen.Name));
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = true,
                Data = siblings.ToList().AsReadOnly()
            };
        }
    }
}