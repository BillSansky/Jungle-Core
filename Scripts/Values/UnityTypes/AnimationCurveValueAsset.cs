using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve Value", fileName = "AnimationCurveValue")]
    public class AnimationCurveValueAsset : ValueAsset<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public override AnimationCurve GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class AnimationCurveValueFromAsset :
        ValueFromAsset<AnimationCurve, AnimationCurveValueAsset>, IAnimationCurveValue
    {
    }
}
