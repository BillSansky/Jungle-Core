using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IAnimationCurveValue : IValue<AnimationCurve>
    {
    }

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

    [Serializable]
    public class AnimationCurveClassMembersValue : ClassMembersValue<AnimationCurve>, IAnimationCurveValue
    {
    }
}
