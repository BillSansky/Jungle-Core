using Jungle.Values;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// MonoBehaviour that serializes a Renderer reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class RendererValueComponent : ValueComponent<Renderer>
    {
        [SerializeField]
        private Renderer value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Renderer Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Renderer value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Renderer references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class RendererListValueComponent : SerializedValueListComponent<Renderer>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Renderer reference from the assigned RendererValueComponent.
    /// </summary>
    [Serializable]
    public class RendererValueFromComponent : ValueFromComponent<Renderer, RendererValueComponent>, IRendererValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Renderer references from a RendererListValueComponent component.
    /// </summary>
    [Serializable]
    public class RendererListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Renderer>, RendererListValueComponent>
    {
    }
}
