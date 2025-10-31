using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes an AnimationClip reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class AnimationClipValueComponent : ValueComponent<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override AnimationClip Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(AnimationClip value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of AnimationClip references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class AnimationClipListValueComponent : SerializedValueListComponent<AnimationClip>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AnimationClip reference from a AnimationClipValueComponent component.
    /// </summary>
    [Serializable]
    public class AnimationClipValueFromComponent :
        ValueFromComponent<AnimationClip, AnimationClipValueComponent>, IAnimationClipValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AnimationClip references from a AnimationClipListValueComponent component.
    /// </summary>
    [Serializable]
    public class AnimationClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationClip>, AnimationClipListValueComponent>
    {
    }
}
