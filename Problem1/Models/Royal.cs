using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Problem1.Exceptions;
using Problem1.Interfaces;

namespace Problem1.Models
{
    [DebuggerDisplay("{Sex} - {Name} - {GenerationId}")]
    public class Royal : IRoyal
    {
        private ICitizen _partner;
        private int _generationId;

        public int GenerationId
        {
            get { return _generationId; }
        }
        public string Name { get; set; }
        public Sex Sex { get; set; }

        private readonly List<IRoyal> _children;

        public Royal(string name, Sex sex)
        {
            Name = name;
            Sex = sex;
            _children = new List<IRoyal>();
        }

        public IRoyal AddPartner(ICitizen partner)
        {
            if (_partner != null)
            {
                throw new DoNotCheatException();
            }

            _partner = partner;
            return this;
        }

        public IRoyal AddChild(string name, Sex sex)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new RoyalDoesNotExist("Please give a name to the child");
            }

            var child = new Royal(name, sex) {_generationId = this.GenerationId + 1};
            _children.Add(child);

            return this;
        }


        public IReadOnlyList<IRoyal> Children
        {
            get { return _children.AsReadOnly(); }
        }
    }

}
