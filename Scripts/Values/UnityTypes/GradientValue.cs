using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IGradientValue : IValue<Gradient>
    {
    }

    [Serializable]
    [JungleClassInfo("Gradient Value", "Stores a gradient locally on the owner.", null, "Values/Unity Types", true)]
    public class GradientValue : LocalValue<Gradient>, IGradientValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Gradient Member Value", "Returns a gradient from a component field, property, or method.", null, "Values/Unity Types")]
    public class GradientClassMembersValue : ClassMembersValue<Gradient>, IGradientValue
    {
    }
}
