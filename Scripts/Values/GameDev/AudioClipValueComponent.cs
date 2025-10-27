using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class AudioClipValueComponent : ValueComponent<AudioClip>
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

    [Serializable]
    public class AudioClipValueFromComponent :
        ValueFromComponent<AudioClip, AudioClipValueComponent>, IAudioClipValue
    {
    }
}
