using Jungle.Values;
using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Component exposing a renderer component.
    /// </summary>
    [JungleClassInfo("Renderer Value Component", "Component exposing a renderer component.", null, "Values/Game Dev")]
    public class RendererValueComponent : ValueComponent<Renderer>
    {
        [SerializeField]
        private Renderer value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Renderer Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Renderer value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of renderers.
    /// </summary>

    [JungleClassInfo("Renderer List Component", "Component exposing a list of renderers.", null, "Values/Game Dev")]
    public class RendererListValueComponent : SerializedValueListComponent<Renderer>
    {
    }
    /// <summary>
    /// Reads a renderer component from a RendererValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Renderer Value From Component", "Reads a renderer component from a RendererValueComponent.", null, "Values/Game Dev")]
    public class RendererValueFromComponent : ValueFromComponent<Renderer, RendererValueComponent>, ISettableRendererValue
    {
    }
    /// <summary>
    /// Reads renderers from a RendererListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Renderer List From Component", "Reads renderers from a RendererListValueComponent.", null, "Values/Game Dev")]
    public class RendererListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Renderer>, RendererListValueComponent>
    {
    }
}
