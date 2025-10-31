using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a GameObject reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class GameObjectValueComponent : ValueComponent<GameObject>
    {
        [SerializeField]
        private GameObject value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override GameObject Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(GameObject value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of GameObject references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class GameObjectListValueComponent : SerializedValueListComponent<GameObject>
    {
    }
    /// <summary>
    /// Value wrapper that reads a GameObject reference from a GameObjectValueComponent component.
    /// </summary>
    [Serializable]
    public class GameObjectValueFromComponent :
        ValueFromComponent<GameObject, GameObjectValueComponent>, IGameObjectValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of GameObject references from a GameObjectListValueComponent component.
    /// </summary>
    [Serializable]
    public class GameObjectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<GameObject>, GameObjectListValueComponent>
    {
    }
}
