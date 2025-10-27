using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class AnimationClipValueComponent : ValueComponent<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;

        public override AnimationClip Value()
        {
            return value;
        }

        public override void SetValue(AnimationClip value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class AnimationClipValueFromComponent :
        ValueFromComponent<AnimationClip, AnimationClipValueComponent>, IAnimationClipValue
    {
    }
}
