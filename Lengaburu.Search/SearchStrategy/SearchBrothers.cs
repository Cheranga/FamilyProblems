using System;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchBrothers : SearchSiblings
    {
        protected override string NotFoundMessage
        {
            get { return "There are no brothers"; }
        }

        protected override Func<ICitizen, bool> Filter
        {
            get { return x => x.Sex == Sex.Male; }
        }

        public override string Name
        {
            get { return "Brother"; }
        }
    }
}