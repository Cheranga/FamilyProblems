using System;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchSisterInLaws : SearchInLawSiblings
    {
        protected override string NotFoundMessage
        {
            get { return "There are no sister in laws"; }
        }

        protected override Func<ICitizen, bool> Filter
        {
            get { return x => x.Sex == Sex.Female; }
        }

        public override string Name
        {
            get { return "Sister in Law"; }
        }
    }
}