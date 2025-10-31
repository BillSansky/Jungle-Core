using System;
using System.Collections.Generic;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the ITransformValue contract.
    /// </summary>
    public interface ITransformValue : IComponent<Transform>
    {
      
        public Vector3 Position => ((IValue<Transform>)this).Value().position;
        public Vector3 LocalScale => ((IValue<Transform>)this).Value().localScale;
        public Quaternion Rotation => ((IValue<Transform>)this).Value().rotation;
        
    }
    /// <summary>
    /// Stores a Transform reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class TransformLocalValue : LocalValue<Transform>, ITransformValue
    {
        public override bool HasMultipleValues => false;

       
    }
    /// <summary>
    /// Resolves a Transform reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class TransformClassMembersValue : ClassMembersValue<Transform>, ITransformValue
    {
    }
}
