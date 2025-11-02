using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values
{
    [Serializable]
    [JungleClassInfo("Renderer Array Value", "Provides renderer references from a serialized array.", null, "Values/Game Dev", true)]
    public class RendererLocalArrayValue : LocalArrayValue<Renderer>, IRendererValue
    {
    }
}
