using System;
using UnityEngine;
using Jungle.Values.Context;
using Jungle.Values.GameDev;

namespace Jungle.Values.Context.Impl
{
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