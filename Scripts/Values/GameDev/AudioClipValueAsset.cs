using System;
using System.Collections.Generic;
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

        public override void SetValue(AudioClip value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip list value", fileName = "AudioClipListValue")]
    public class AudioClipListValueAsset : SerializedValueListAsset<AudioClip>
    {
    }

    [Serializable]
    public class AudioClipValueFromAsset :
        ValueFromAsset<AudioClip, AudioClipValueAsset>, IAudioClipValue
    {
    }

    [Serializable]
    public class AudioClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioClip>, AudioClipListValueAsset>
    {
    }
}
