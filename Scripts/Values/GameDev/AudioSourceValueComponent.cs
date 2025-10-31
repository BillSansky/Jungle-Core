using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes an AudioSource reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class AudioSourceValueComponent : ValueComponent<AudioSource>
    {
        [SerializeField]
        private AudioSource value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override AudioSource Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(AudioSource value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of AudioSource references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class AudioSourceListValueComponent : SerializedValueListComponent<AudioSource>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AudioSource reference from a AudioSourceValueComponent component.
    /// </summary>
    [Serializable]
    public class AudioSourceValueFromComponent :
        ValueFromComponent<AudioSource, AudioSourceValueComponent>, IAudioSourceValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AudioSource references from a AudioSourceListValueComponent component.
    /// </summary>
    [Serializable]
    public class AudioSourceListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioSource>, AudioSourceListValueComponent>
    {
    }
}
