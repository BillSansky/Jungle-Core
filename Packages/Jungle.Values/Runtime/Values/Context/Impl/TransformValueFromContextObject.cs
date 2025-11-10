using System;
using UnityEngine;
using Jungle.Values.Context;
using Jungle.Values.GameDev;

namespace Jungle.Values.Context.Impl
{
    /// <summary>
    /// Extracts a Transform reference from a context-supplied object.
    /// </summary>
    [Serializable]
    public class TransformValueFromContextObject : ComponentValueFromGameObjectContext<Transform>, ITransformValue
    {
    }
}
