using System;
using System.Collections.Generic;
using Lengaburu.Core.Models;

namespace Lengaburu.Core.Interfaces
{
    public interface ICitizen : IEquatable<ICitizen>
    {
        int GenerationLevel { get; set; }
        string Name { get; set; }
        Sex Sex { get; set; }
        ICitizen Father { get; set; }
        ICitizen Mother { get; set; }
        ICitizen Partner { get;}

        IReadOnlyList<ICitizen> Children { get; }

        ICitizen AddPartner(ICitizen partner);
        ICitizen AddPartner(string name, Sex sex);
        ICitizen AddChild(string name, Sex sex);
        ICitizen AddChild(ICitizen child);
    }
}