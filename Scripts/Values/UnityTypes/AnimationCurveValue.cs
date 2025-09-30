using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IAnimationCurveValue : IValue<AnimationCurve>
    {
    }

    [Serializable]
    public class AnimationCurveValue : LocalValue<AnimationCurve>, IAnimationCurveValue
    {
    }
}
