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
}
