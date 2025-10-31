using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ITransformValue : IComponent<Transform>
    {
      
        public Vector3 Position => ((IValue<Transform>)this).Value().position;
        public Vector3 LocalScale => ((IValue<Transform>)this).Value().localScale;
        public Quaternion Rotation => ((IValue<Transform>)this).Value().rotation;
        
    }

    [Serializable]
    [JungleClassInfo("Transform Value", "Stores a transform component directly on the owner.", null, "Values/Game Dev", true)]
    public class TransformLocalValue : LocalValue<Transform>, ITransformValue
    {
        public override bool HasMultipleValues => false;

       
    }

    [Serializable]
    [JungleClassInfo("Transform Member Value", "Returns a transform component from a component field, property, or method.", null, "Values/Game Dev")]
    public class TransformClassMembersValue : ClassMembersValue<Transform>, ITransformValue
    {
    }
}
