using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing an audio source component.
    /// </summary>
    [JungleClassInfo("Audio Source Value Component", "Component exposing an audio source component.", null, "Values/Game Dev")]
    public class AudioSourceValueComponent : ValueComponent<AudioSource>
    {
        [SerializeField]
        private AudioSource value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override AudioSource Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(AudioSource value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of audio sources.
    /// </summary>

    [JungleClassInfo("Audio Source List Component", "Component exposing a list of audio sources.", null, "Values/Game Dev")]
    public class AudioSourceListValueComponent : SerializedValueListComponent<AudioSource>
    {
    }
    /// <summary>
    /// Reads an audio source component from an AudioSourceValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Source Value From Component", "Reads an audio source component from an AudioSourceValueComponent.", null, "Values/Game Dev")]
    public class AudioSourceValueFromComponent :
        ValueFromComponent<AudioSource, AudioSourceValueComponent>, ISettableAudioSourceValue
    {
    }
    /// <summary>
    /// Reads audio sources from an AudioSourceListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Source List From Component", "Reads audio sources from an AudioSourceListValueComponent.", null, "Values/Game Dev")]
    public class AudioSourceListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioSource>, AudioSourceListValueComponent>
    {
    }
}
