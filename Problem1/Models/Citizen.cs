using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lengaburu.Core.Exceptions;
using Lengaburu.Core.Interfaces;

namespace Lengaburu.Core.Models
{
    [DebuggerDisplay("{GenerationLevel} - {Name} - {Sex}")]
    public class Citizen : ICitizen
    {
        private ICitizen _partner;
        private readonly List<ICitizen> _children;

        public Citizen(string name, Sex sex)
        {
            Name = name;
            Sex = sex;
            _children = new List<ICitizen>();
        }


        public int GenerationLevel { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public ICitizen Father { get; set; }
        public ICitizen Mother { get; set; }

        public ICitizen Partner
        {
            get
            {
                return _partner;
            }
        }

        public IReadOnlyList<ICitizen> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public ICitizen AddPartner(string name, Sex sex)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new PersonDoesNotExist();
            }

            return AddPartner(new Citizen(name, sex));
        }

        public ICitizen AddPartner(ICitizen partner)
        {
            if (Partner != null)
            {
                throw new DoNotCheatException();
            }

            _partner = partner;
            partner.GenerationLevel = this.GenerationLevel;

            return this;
        }

        public ICitizen AddChild(string name, Sex sex)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new PersonDoesNotExist();
            }

            var child = new Citizen(name, sex);
            return AddChild(child);
        }

        public ICitizen AddChild(ICitizen child)
        {
            if (child == null)
            {
                throw new PersonDoesNotExist();
            }

            child.GenerationLevel = GenerationLevel + 1;
            _children.Add(child);

            return this;
        }

        public bool Equals(ICitizen other)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(other?.Name))
            {
                return false;
            }

            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}