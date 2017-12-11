using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.SearchStrategy
{
    public class SearchBrotherInLaws : SearchInLawSiblings
    {
        public override Status<IReadOnlyList<ICitizen>> Find(ICitizen citizen)
        {
            var status = base.Find(citizen);
            if (status.IsValid == false)
            {
                return status;
            }

            var inlaws = status.Data.Where(x => x.Sex == Sex.Male).ToList();
            if (inlaws.Any())
            {
                return new Status<IReadOnlyList<ICitizen>>
                {
                    IsValid = true,
                    Data = new ReadOnlyCollection<ICitizen>(inlaws)
                };
            }

            return new Status<IReadOnlyList<ICitizen>>
            {
                IsValid = false,
                Message = "There are no brother in laws"
            };
        }
    }
}