using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve value", fileName = "AnimationCurveValue")]
    public class AnimationCurveValueAsset : ValueAsset<AnimationCurve>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve list value", fileName = "AnimationCurveListValue")]
    public class AnimationCurveListValueAsset : SerializedValueListAsset<AnimationCurve>
    {
    }

    [Serializable]
    public class AnimationCurveValueFromAsset :
        ValueFromAsset<AnimationCurve, AnimationCurveValueAsset>, IAnimationCurveValue
    {
    }

    [Serializable]
    public class AnimationCurveListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationCurve>, AnimationCurveListValueAsset>
    {
    }
}
