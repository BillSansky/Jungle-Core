using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing an AudioSource reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioSource value", fileName = "AudioSourceLocalValue")]
    public class AudioSourceValueAsset : ValueAsset<AudioSource>
    {
        [SerializeField]
        private AudioSource value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override AudioSource Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(AudioSource value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of AudioSource references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioSource list value", fileName = "AudioSourceListValue")]
    public class AudioSourceListValueAsset : SerializedValueListAsset<AudioSource>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AudioSource reference from an assigned AudioSourceValueAsset.
    /// </summary>
    [Serializable]
    public class AudioSourceValueFromAsset :
        ValueFromAsset<AudioSource, AudioSourceValueAsset>, IAudioSourceValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AudioSource references from an assigned AudioSourceListValueAsset.
    /// </summary>
    [Serializable]
    public class AudioSourceListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioSource>, AudioSourceListValueAsset>
    {
    }
}
