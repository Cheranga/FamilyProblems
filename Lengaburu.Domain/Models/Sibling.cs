using System.Collections.Generic;
using Problem1.Interfaces;

namespace Problem1.Models
{
    public class Sibling
    {
        private readonly ICitizen _citizen;
        private readonly IEnumerable<ICitizen> _siblings;

        public Sibling(ICitizen citizen, IEnumerable<ICitizen> siblings )
        {
            _citizen = citizen;
            _siblings = siblings;
        }
    }
}