using System;
using UnityEngine;
using Jungle.Values.Context;
using Jungle.Values.GameDev;

namespace Jungle.Values.Context.Impl
{
    /// <summary>
    /// Resolves a Transform reference from the context object lookup.
    /// </summary>
    [Serializable]
    public class TransformValueFromContextObject : ValueFromObjectContext<Transform>, ITransformValue
    {
        [SerializeField]
        private int contextId;

        [SerializeField]
        private ComponentRetrievalStrategy strategy;

        public override ComponentRetrievalStrategy Strategy => strategy;
    }
}