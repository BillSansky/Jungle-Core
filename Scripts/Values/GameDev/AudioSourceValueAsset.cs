using System;
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
    }

    [Serializable]
    public class AudioSourceValueFromAsset :
        ValueFromAsset<AudioSource, AudioSourceValueAsset>, IAudioSourceValue
    {
    }
}
