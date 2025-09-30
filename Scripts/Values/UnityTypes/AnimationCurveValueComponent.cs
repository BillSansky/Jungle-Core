using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class AnimationCurveValueComponent : ValueComponent<AnimationCurve>, IAnimationCurveValue
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public override AnimationCurve GetValue()
        {
            return value;
        }
    }
}
