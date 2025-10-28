using System;
using System.Collections.Generic;
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

    public class AnimationClipListValueComponent : SerializedValueListComponent<AnimationClip>
    {
    }

    [Serializable]
    public class AnimationClipValueFromComponent :
        ValueFromComponent<AnimationClip, AnimationClipValueComponent>, IAnimationClipValue
    {
    }

    [Serializable]
    public class AnimationClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationClip>, AnimationClipListValueComponent>
    {
    }
}
