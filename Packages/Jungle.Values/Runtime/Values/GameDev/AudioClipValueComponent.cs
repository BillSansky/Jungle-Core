using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing an audio clip.
    /// </summary>
    [JungleClassInfo("Audio Clip Value Component", "Component exposing an audio clip.", null, "Game Dev")]
    public class AudioClipValueComponent : ValueComponent<AudioClip>
    {
        [SerializeField]
        private AudioClip value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override AudioClip Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(AudioClip value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of audio clips.
    /// </summary>

    [JungleClassInfo("Audio Clip List Component", "Component exposing a list of audio clips.", null, "Game Dev")]
    public class AudioClipListValueComponent : SerializedValueListComponent<AudioClip>
    {
    }
    /// <summary>
    /// Reads an audio clip from an AudioClipValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Clip Value From Component", "Reads an audio clip from an AudioClipValueComponent.", null, "Game Dev")]
    public class AudioClipValueFromComponent :
        ValueFromComponent<AudioClip, AudioClipValueComponent>, ISettableAudioClipValue
    {
    }
    /// <summary>
    /// Reads audio clips from an AudioClipListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Clip List From Component", "Reads audio clips from an AudioClipListValueComponent.", null, "Game Dev")]
    public class AudioClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioClip>, AudioClipListValueComponent>
    {
    }
}
