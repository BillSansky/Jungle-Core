using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns a Material reference.
    /// </summary>
    public interface IMaterialValue : IValue<Material>
    {
    }
    public interface ISettableMaterialValue : IMaterialValue, IValueSableValue<Material>
    {
    }
    /// <summary>
    /// Stores a material reference directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Material Value", "Stores a material reference directly on the owner.", null, "Game Dev", true)]
    public class MaterialValue : LocalValue<Material>, ISettableMaterialValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a material reference from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Material Member Value", "Returns a material reference from a component field, property, or method.", null, "Game Dev")]
    public class MaterialClassMembersValue : ClassMembersValue<Material>, IMaterialValue
    {
    }
}
