using System.Collections.Generic;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.SearchStrategy
{
    public class IdentifierByName : IUniqueIdentifier<string, ICitizen>
    {
        public Status<ICitizen> FindAll(IEnumerable<ICitizen> citizens, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return new Status<ICitizen>
                {
                    IsValid = false,
                    Message = "Please provide a name to search"
                };
            }

            search = search.ToUpper();
            var list = citizens as List<ICitizen> ?? new List<ICitizen>();
            if (list.Any() == false)
            {
                return new Status<ICitizen>
                {
                    IsValid = false,
                    Message = "Please provide citizens to perform search"
                };
            }

            var citizensMappedByName = citizens.GroupBy(x => x.Name)
                .ToDictionary(x => x.Key.ToUpper(), x => new { Count = x.Count(), Citizens = x.ToList() });

            if (citizensMappedByName.ContainsKey(search) == false)
            {
                return new Status<ICitizen>
                {
                    IsValid = false,
                    Message = $"There is no person by the name '[{search}]'"
                };
            }

            var foundCitizens = citizensMappedByName[search];
            if (foundCitizens.Count > 1)
            {
                return new Status<ICitizen>
                {
                    IsValid = false,
                    Message = $"There are more than one person by the name [{search}]"
                };
            }

            return new Status<ICitizen>
            {
                IsValid = true,
                Data = foundCitizens.Citizens.First()
            };
        }
    }
}