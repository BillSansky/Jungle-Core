using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Audio Source Value Component", "Component exposing an audio source component.", null, "Values/Game Dev")]
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

    [JungleClassInfo("Audio Source List Component", "Component exposing a list of audio sources.", null, "Values/Game Dev")]
    public class AudioSourceListValueComponent : SerializedValueListComponent<AudioSource>
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Source Value From Component", "Reads an audio source component from an AudioSourceValueComponent.", null, "Values/Game Dev")]
    public class AudioSourceValueFromComponent :
        ValueFromComponent<AudioSource, AudioSourceValueComponent>, IAudioSourceValue
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Source List From Component", "Reads audio sources from an AudioSourceListValueComponent.", null, "Values/Game Dev")]
    public class AudioSourceListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioSource>, AudioSourceListValueComponent>
    {
    }
}
