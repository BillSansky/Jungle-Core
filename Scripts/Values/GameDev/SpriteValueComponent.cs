using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a Sprite reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class SpriteValueComponent : ValueComponent<Sprite>
    {
        [SerializeField]
        private Sprite value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Sprite Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Sprite value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Sprite references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class SpriteListValueComponent : SerializedValueListComponent<Sprite>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Sprite reference from a SpriteValueComponent component.
    /// </summary>
    [Serializable]
    public class SpriteValueFromComponent :
        ValueFromComponent<Sprite, SpriteValueComponent>, ISpriteValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Sprite references from a SpriteListValueComponent component.
    /// </summary>
    [Serializable]
    public class SpriteListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Sprite>, SpriteListValueComponent>
    {
    }
}
