using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [Serializable]
    public class PositionFromTransformValue : IVector3Value
    {

        [SerializeReference] [JungleClassSelection]
        public ITransformValue transform;
        
        public Vector3 Value()
        {
            return transform.V.position;
        }

        public bool HasMultipleValues =>transform.HasMultipleValues;

        public IEnumerable<Vector3> Values
        {
            get
            {
                foreach (var transformValue in transform.Values)
                {
                    yield return transformValue.position;
                }
            }
        }
    }
}