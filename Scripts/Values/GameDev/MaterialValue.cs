using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IMaterialValue : IValue<Material>
    {
    }

    [Serializable]
    [JungleClassInfo("Material Value", "Stores a material reference directly on the owner.", null, "Values/Game Dev", true)]
    public class MaterialValue : LocalValue<Material>, IMaterialValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Material Member Value", "Returns a material reference from a component field, property, or method.", null, "Values/Game Dev")]
    public class MaterialClassMembersValue : ClassMembersValue<Material>, IMaterialValue
    {
    }
}
