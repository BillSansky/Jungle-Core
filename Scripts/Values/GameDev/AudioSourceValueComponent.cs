using System;
using System.Collections.Generic;
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

    public class AudioSourceListValueComponent : SerializedValueListComponent<AudioSource>
    {
    }

    [Serializable]
    public class AudioSourceValueFromComponent :
        ValueFromComponent<AudioSource, AudioSourceValueComponent>, IAudioSourceValue
    {
    }

    [Serializable]
    public class AudioSourceListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioSource>, AudioSourceListValueComponent>
    {
    }
}
