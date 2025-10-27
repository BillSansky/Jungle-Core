using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class AudioSourceValueComponent : ValueComponent<AudioSource>
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

    [Serializable]
    public class AudioSourceValueFromComponent :
        ValueFromComponent<AudioSource, AudioSourceValueComponent>, IAudioSourceValue
    {
    }
}
