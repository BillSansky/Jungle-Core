using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class AudioClipValueComponent : ValueComponent<AudioClip>
    {
        [SerializeField]
        private AudioClip value;

        public override AudioClip GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class AudioClipValueFromComponent :
        ValueFromComponent<AudioClip, AudioClipValueComponent>, IAudioClipValue
    {
    }
}
