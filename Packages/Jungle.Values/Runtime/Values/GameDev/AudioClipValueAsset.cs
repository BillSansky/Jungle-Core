using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing an audio clip.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip value", fileName = "AudioClipValue")]
    [JungleClassInfo("Audio Clip Value Asset", "ScriptableObject storing an audio clip.", null, "Game Dev")]
    public class AudioClipValueAsset : ValueAsset<AudioClip>
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
    /// ScriptableObject storing a list of audio clips.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip list value", fileName = "AudioClipListValue")]
    [JungleClassInfo("Audio Clip List Asset", "ScriptableObject storing a list of audio clips.", null, "Game Dev")]
    public class AudioClipListValueAsset : SerializedValueListAsset<AudioClip>
    {
    }
    /// <summary>
    /// Reads an audio clip from an AudioClipValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Clip Value From Asset", "Reads an audio clip from an AudioClipValueAsset.", null, "Game Dev")]
    public class AudioClipValueFromAsset :
        ValueFromAsset<AudioClip, AudioClipValueAsset>, ISettableAudioClipValue
    {
    }
    /// <summary>
    /// Reads audio clips from an AudioClipListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Clip List From Asset", "Reads audio clips from an AudioClipListValueAsset.", null, "Game Dev")]
    public class AudioClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioClip>, AudioClipListValueAsset>
    {
    }
}
