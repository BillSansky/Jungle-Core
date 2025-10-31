using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IAnimationCurveValue contract.
    /// </summary>
    public interface IAnimationCurveValue : IValue<AnimationCurve>
    {
    }
    /// <summary>
    /// Stores an AnimationCurve directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class AnimationCurveValue : LocalValue<AnimationCurve>, IAnimationCurveValue
    {
        public AnimationCurveValue()
        {
        }

        public AnimationCurveValue(AnimationCurve value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Resolves an AnimationCurve by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class AnimationCurveClassMembersValue : ClassMembersValue<AnimationCurve>, IAnimationCurveValue
    {
    }
}
