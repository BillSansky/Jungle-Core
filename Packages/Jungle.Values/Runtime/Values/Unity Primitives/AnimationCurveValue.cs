using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns an AnimationCurve value.
    /// </summary>
    public interface IAnimationCurveValue : IValue<AnimationCurve>
    {
    }
    public interface ISettableAnimationCurveValue : IAnimationCurveValue, IValueSableValue<AnimationCurve>
    {
    }
    /// <summary>
    /// Stores a animation curve locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("AnimationCurve Value", "Stores a animation curve locally on the owner.", null, "Values/Unity Primitives", true)]
    public class AnimationCurveValue : LocalValue<AnimationCurve>, ISettableAnimationCurveValue
    {
        /// <summary>
        /// Initializes a new instance of the AnimationCurveValue.
        /// </summary>
        public AnimationCurveValue()
        {
        }
        /// <summary>
        /// Initializes a new instance of the AnimationCurveValue.
        /// </summary>

        public AnimationCurveValue(AnimationCurve value)
            : base(value)
        {
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Returns a animation curve from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("AnimationCurve Member Value", "Returns a animation curve from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class AnimationCurveClassMembersValue : ClassMembersValue<AnimationCurve>, IAnimationCurveValue
    {
    }
}
