using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IAnimationCurveValue : IValue<AnimationCurve>
    {
    }

    [Serializable]
    [JungleClassInfo("AnimationCurve Value", "Stores a animation curve locally on the owner.", null, "Values/Unity Types", true)]
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
    [JungleClassInfo("AnimationCurve Member Value", "Returns a animation curve from a component field, property, or method.", null, "Values/Unity Types")]
    public class AnimationCurveClassMembersValue : ClassMembersValue<AnimationCurve>, IAnimationCurveValue
    {
    }
}
