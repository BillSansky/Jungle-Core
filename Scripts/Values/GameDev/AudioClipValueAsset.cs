using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip Value", fileName = "AudioClipValue")]
    public class AudioClipValueAsset : ValueAsset<AudioClip>, IAudioClipValue
    {
        [SerializeField]
        private AudioClip value;

        public override AudioClip GetValue()
        {
            return value;
        }
    }
}
