using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IMaterialValue contract.
    /// </summary>
    public interface IMaterialValue : IValue<Material>
    {
    }
    /// <summary>
    /// Stores a Material reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class MaterialValue : LocalValue<Material>, IMaterialValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Material reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class MaterialClassMembersValue : ClassMembersValue<Material>, IMaterialValue
    {
    }
}
