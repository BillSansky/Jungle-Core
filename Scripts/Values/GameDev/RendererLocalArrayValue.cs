using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Stores a locally serialized array of Renderer references for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class RendererLocalArrayValue : LocalArrayValue<Renderer>, IRendererValue
    {
    }
}
