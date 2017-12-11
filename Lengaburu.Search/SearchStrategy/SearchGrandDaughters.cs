using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lengaburu.Core.Interfaces;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Search.SearchStrategy
{
    public class SearchGrandDaughters : SearchGrandChildren
    {
        protected override string NotFoundMessage
        {
            get
            {
                return "There are no grand daughters";
            }
        }

        protected override Func<ICitizen, bool> Filter
        {
            get { return x => x.Sex == Sex.Female; }
        }
    }
}