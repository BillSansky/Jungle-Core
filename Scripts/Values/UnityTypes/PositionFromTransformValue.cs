﻿using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Reads a Transform position and exposes it as a reusable value.
    /// </summary>
    [Serializable]
    public class PositionFromTransformValue : IVector3Value
    {

        [SerializeReference] [JungleClassSelection]
        public ITransformValue transform;
        /// <summary>
        /// Returns the world position of the referenced transform value.
        /// </summary>
        public Vector3 Value()
        {
            return transform.Position;
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