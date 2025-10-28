using System;
using System.Collections.Generic;
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

    public class AudioClipListValueComponent : SerializedValueListComponent<AudioClip>
    {
    }

    [Serializable]
    public class AudioClipValueFromComponent :
        ValueFromComponent<AudioClip, AudioClipValueComponent>, IAudioClipValue
    {
    }

    [Serializable]
    public class AudioClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioClip>, AudioClipListValueComponent>
    {
    }
}
