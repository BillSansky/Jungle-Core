using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a Component reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class ComponentValueComponent : ValueComponent<Component>
    {
        [SerializeField]
        private Component value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Component Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Component value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Component references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class ComponentListValueComponent : SerializedValueListComponent<Component>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Component reference from the assigned ComponentValueComponent.
    /// </summary>
    [Serializable]
    public class ComponentValueFromComponent : ValueFromComponent<Component, ComponentValueComponent>, IComponentValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Component references from a ComponentListValueComponent component.
    /// </summary>
    [Serializable]
    public class ComponentListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Component>, ComponentListValueComponent>
    {
    }
}
