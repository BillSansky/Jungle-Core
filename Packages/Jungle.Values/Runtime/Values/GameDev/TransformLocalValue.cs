using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to a Transform reference.
    /// </summary>
    public interface ITransformValue : IValue<Transform>
    {
        /// <summary>
        /// Gets the world position of the referenced transform.
        /// </summary>

        public Vector3 Position => ((IValue<Transform>)this).Value().position;
        /// <summary>
        /// Gets the local scale of the referenced transform.
        /// </summary>
        public Vector3 LocalScale => ((IValue<Transform>)this).Value().localScale;
        /// <summary>
        /// Gets the world rotation of the referenced transform.
        /// </summary>
        public Quaternion Rotation => ((IValue<Transform>)this).Value().rotation;

    }
    public interface ISettableTransformValue : ITransformValue, IValueSableValue<Transform>
    {
    }
    /// <summary>
    /// Stores a transform component directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Transform Value", "Stores a transform component directly on the owner.", null, "Values/Game Dev", true)]
    public class TransformLocalValue : LocalValue<Transform>, ISettableTransformValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;

       
    }
    /// <summary>
    /// Returns a transform component from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Transform Member Value", "Returns a transform component from a component field, property, or method.", null, "Values/Game Dev")]
    public class TransformClassMembersValue : ClassMembersValue<Transform>, ITransformValue
    {
    }
}
