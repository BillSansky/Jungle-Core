using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing an AudioClip reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip value", fileName = "AudioClipValue")]
    public class AudioClipValueAsset : ValueAsset<AudioClip>
    {
        [SerializeField]
        private AudioClip value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override AudioClip Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(AudioClip value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of AudioClip references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip list value", fileName = "AudioClipListValue")]
    public class AudioClipListValueAsset : SerializedValueListAsset<AudioClip>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AudioClip reference from an assigned AudioClipValueAsset.
    /// </summary>
    [Serializable]
    public class AudioClipValueFromAsset :
        ValueFromAsset<AudioClip, AudioClipValueAsset>, IAudioClipValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AudioClip references from an assigned AudioClipListValueAsset.
    /// </summary>
    [Serializable]
    public class AudioClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioClip>, AudioClipListValueAsset>
    {
    }
}
