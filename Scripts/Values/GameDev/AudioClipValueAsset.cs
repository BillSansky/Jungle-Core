using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip value", fileName = "AudioClipValue")]
    public class AudioClipValueAsset : ValueAsset<AudioClip>
    {
        [SerializeField]
        private AudioClip value;

        public override AudioClip Value()
        {
            return value;
        }
    }

    [Serializable]
    public class AudioClipValueFromAsset :
        ValueFromAsset<AudioClip, AudioClipValueAsset>, IAudioClipValue
    {
    }
}
