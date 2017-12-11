using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Problem1.Interfaces;
using Problem1.Models;

namespace Problem1.SearchStrategy
{
    public class SearchGrandSons : SearchGrandChildren
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var grandSons = status.Data.Where(x => x.Sex == Sex.Male).ToList();
            if (grandSons.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(grandSons)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no grand sons"
            };
        }
    }
}