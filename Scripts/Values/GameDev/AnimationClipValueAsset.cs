using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip value", fileName = "AnimationClipValue")]
    public class AnimationClipValueAsset : ValueAsset<AnimationClip>
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip list value", fileName = "AnimationClipListValue")]
    public class AnimationClipListValueAsset : SerializedValueListAsset<AnimationClip>
    {
    }

    [Serializable]
    public class AnimationClipValueFromAsset :
        ValueFromAsset<AnimationClip, AnimationClipValueAsset>, IAnimationClipValue
    {
    }

    [Serializable]
    public class AnimationClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationClip>, AnimationClipListValueAsset>
    {
    }
}
