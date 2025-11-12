using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides renderer references from a serialized array.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Renderer Array Value", "Provides renderer references from a serialized array.", null, "Game Dev", true)]
    public class RendererLocalArrayValue : LocalArrayValue<Renderer>, IRendererValue
    {
    }
}
