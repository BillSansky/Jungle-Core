using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IRectValue : IValue<Rect>
    {
    }

    [Serializable]
    [JungleClassInfo("Rect Value", "Stores a Rect area locally on the owner.", null, "Values/Unity Types", true)]
    public class RectValue : LocalValue<Rect>, IRectValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Rect Member Value", "Returns a Rect area from a component field, property, or method.", null, "Values/Unity Types")]
    public class RectClassMembersValue : ClassMembersValue<Rect>, IRectValue
    {
    }
}
