using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves a Transform component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Transform Lookup Value", "Retrieves a Transform component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Game Dev")]
    public class TransformLookupValue : BaseComponentLookupValue<Transform, ITransformValue>, ITransformValue
    {
    }
}
