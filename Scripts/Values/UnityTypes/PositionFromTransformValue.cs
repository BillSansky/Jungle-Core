﻿using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [Serializable]
    [JungleClassInfo("Position From Transform", "Reads the world position from a referenced transform value.", null, "Values/Unity Types")]
    public class PositionFromTransformValue : IVector3Value
    {

        [SerializeReference] [JungleClassSelection]
        public ITransformValue transform;
        
        public Vector3 Value()
        {
            return transform.Position;
        }

        public bool HasMultipleValues => transform.HasMultipleValues;

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