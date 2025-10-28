using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioSource value", fileName = "AudioSourceLocalValue")]
    public class AudioSourceValueAsset : ValueAsset<AudioSource>
    {
        [SerializeField]
        private AudioSource value;

        public override AudioSource Value()
        {
            return value;
        }

        public override void SetValue(AudioSource value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioSource list value", fileName = "AudioSourceListValue")]
    public class AudioSourceListValueAsset : SerializedValueListAsset<AudioSource>
    {
    }

    [Serializable]
    public class AudioSourceValueFromAsset :
        ValueFromAsset<AudioSource, AudioSourceValueAsset>, IAudioSourceValue
    {
    }

    [Serializable]
    public class AudioSourceListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioSource>, AudioSourceListValueAsset>
    {
    }
}
