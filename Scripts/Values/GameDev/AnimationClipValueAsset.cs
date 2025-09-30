using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip Value", fileName = "AnimationClipValue")]
    public class AnimationClipValueAsset : ValueAsset<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;

        public override AnimationClip GetValue()
        {
            return value;
        }
    }
}
