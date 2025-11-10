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
    public class TransformValueFromContextObject : ValueFromObjectContext<Transform>, ITransformValue
    {
        [SerializeField]
        private int contextId;

        [SerializeField]
        private ComponentRetrievalStrategy strategy;
        /// <summary>
        /// Gets how components are located from the context object.
        /// </summary>

        public override ComponentRetrievalStrategy Strategy => strategy;
    }
}