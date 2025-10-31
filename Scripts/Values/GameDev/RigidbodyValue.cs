using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IRigidbodyValue : IComponent<Rigidbody>
    {
    }

    [Serializable]
    [JungleClassInfo("Rigidbody Value", "Stores a rigidbody component directly on the owner.", null, "Values/Game Dev", true)]
    public class RigidbodyValue : LocalValue<Rigidbody>, IRigidbodyValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Rigidbody Member Value", "Returns a rigidbody component from a component field, property, or method.", null, "Values/Game Dev")]
    public class RigidbodyClassMembersValue : ClassMembersValue<Rigidbody>, IRigidbodyValue
    {
    }
}
