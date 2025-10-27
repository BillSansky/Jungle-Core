using System;
using UnityEngine;
using Jungle.Values.Context;

namespace Jungle.Values.Context.Impl
{
    [Serializable]
    public class TransformValueFromContextObject : ValueFromObjectContext<Transform>
    {
        [SerializeField]
        private int contextId;

        [SerializeField]
        private ComponentRetrievalStrategy strategy;

        public override ComponentRetrievalStrategy Strategy => strategy;
    }
}