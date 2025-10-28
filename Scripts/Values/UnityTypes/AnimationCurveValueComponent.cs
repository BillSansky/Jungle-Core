using System;
using System.Collections.Generic;
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

        public override void SetValue(AnimationCurve value)
        {
            this.value = value;
        }
    }

    public class AnimationCurveListValueComponent : SerializedValueListComponent<AnimationCurve>
    {
    }

    [Serializable]
    public class AnimationCurveValueFromComponent :
        ValueFromComponent<AnimationCurve, AnimationCurveValueComponent>, IAnimationCurveValue
    {
    }

    [Serializable]
    public class AnimationCurveListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationCurve>, AnimationCurveListValueComponent>
    {
    }
}
