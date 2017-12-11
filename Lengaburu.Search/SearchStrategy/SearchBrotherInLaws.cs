using System;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchBrotherInLaws : SearchInLawSiblings
    {
        protected override string NotFoundMessage
        {
            get { return "There are no brother in laws"; }
        }

        protected override Func<ICitizen, bool> Filter
        {
            get { return x => x.Sex == Sex.Male; }
        }

        public override string Name
        {
            get { return "Brother in Law"; }
        }
    }
}