using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes an AudioClip reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class AudioClipValueComponent : ValueComponent<AudioClip>
    {
        [SerializeField]
        private AudioClip value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override AudioClip Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(AudioClip value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of AudioClip references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class AudioClipListValueComponent : SerializedValueListComponent<AudioClip>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AudioClip reference from a AudioClipValueComponent component.
    /// </summary>
    [Serializable]
    public class AudioClipValueFromComponent :
        ValueFromComponent<AudioClip, AudioClipValueComponent>, IAudioClipValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AudioClip references from a AudioClipListValueComponent component.
    /// </summary>
    [Serializable]
    public class AudioClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioClip>, AudioClipListValueComponent>
    {
    }
}
