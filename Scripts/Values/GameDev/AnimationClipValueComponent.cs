using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class AnimationClipValueComponent : ValueComponent<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;

        public override AnimationClip GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class AnimationClipValueFromComponent :
        ValueFromComponent<AnimationClip, AnimationClipValueComponent>, IAnimationClipValue
    {
    }
}
