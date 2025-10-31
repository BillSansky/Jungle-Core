using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IBoundsValue : IValue<Bounds>
    {
    }

    [Serializable]
    [JungleClassInfo("Bounds Value", "Stores a Bounds value locally on the owner.", null, "Values/Unity Types", true)]
    public class BoundsValue : LocalValue<Bounds>, IBoundsValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Bounds Member Value", "Returns a Bounds value from a component field, property, or method.", null, "Values/Unity Types")]
    public class BoundsClassMembersValue : ClassMembersValue<Bounds>, IBoundsValue
    {
    }
}
