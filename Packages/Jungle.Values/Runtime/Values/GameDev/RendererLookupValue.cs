using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves a Renderer component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Renderer Lookup Value", "Retrieves a Renderer component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Game Dev")]
    public class RendererLookupValue : BaseComponentLookupValue<Renderer, IRendererValue>, IRendererValue
    {
    }
}
