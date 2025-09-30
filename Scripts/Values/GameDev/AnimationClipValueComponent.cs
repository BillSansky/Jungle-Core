using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class AnimationClipValueComponent : ValueComponent<AnimationClip>, IAnimationClipValue
    {
        [SerializeField]
        private AnimationClip value;

        public override AnimationClip GetValue()
        {
            return value;
        }
    }
}
