using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class AnimationCurveValueComponent : ValueComponent<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public override AnimationCurve Value()
        {
            return value;
        }
    }

    [Serializable]
    public class AnimationCurveValueFromComponent :
        ValueFromComponent<AnimationCurve, AnimationCurveValueComponent>, IAnimationCurveValue
    {
    }
}
