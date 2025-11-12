﻿using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Reads the world position from a referenced transform value.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Position From Transform", "Reads the world position from a referenced transform value.", null, "Values/Unity Primitives")]
    public class PositionFromTransformValue : IVector3Value
    {
        /// <summary>
        /// References the transform value used for calculations.
        /// </summary>

        [SerializeReference] [JungleClassSelection]
        public ITransformValue transform = new TransformLocalValue();
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>
        
        public Vector3 Value()
        {
            return transform.Position;
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public bool HasMultipleValues => transform.HasMultipleValues;
        /// <summary>
        /// Enumerates all available values from the provider.
        /// </summary>

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