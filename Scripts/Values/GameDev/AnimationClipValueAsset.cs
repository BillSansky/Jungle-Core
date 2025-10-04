using System;
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
    }

    [Serializable]
    public class AnimationClipValueFromAsset :
        ValueFromAsset<AnimationClip, AnimationClipValueAsset>, IAnimationClipValue
    {
    }
}
