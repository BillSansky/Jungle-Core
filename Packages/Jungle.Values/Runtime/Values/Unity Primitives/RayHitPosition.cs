using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Computes a drag target point by casting a physics ray and using the hit position or a fallback distance when nothing is hit.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Ray Drag Target",
        "Casts a ray and returns the hit position or a fallback point along the ray for drag interactions.",
        null,
        "Values/Unity Types")]
    public class RayHitPosition : IVector3Value
    {
        /// <summary>
        /// Provides the ray used for the drag computation.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection(typeof(IRayValue))]
        public IRayValue ray = new RayValue();

        /// <summary>
        /// Provides the layer mask used when casting the ray.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection(typeof(ILayerMaskValue))]
        public ILayerMaskValue layerMask = new LayerMaskValue();

        /// <summary>
        /// Maximum distance considered for the raycast.
        /// </summary>
        [SerializeField]
        private float maxDistance = 100f;

        /// <summary>
        /// Distance used along the ray direction when no collider is hit.
        /// </summary>
        [SerializeField]
        private float distanceWhenNoHit = 5f;

        /// <inheritdoc />
        public Vector3 Value()
        {
            Ray rayValue = ray.Value();
            LayerMask mask = layerMask.Value();
            return CalculateTarget(rayValue, mask);
        }

        /// <inheritdoc />
        public bool HasMultipleValues => ray.HasMultipleValues || layerMask.HasMultipleValues;

        /// <inheritdoc />
        public IEnumerable<Vector3> Values
        {
            get
            {
                foreach (Ray rayValue in ray.Values)
                {
                    foreach (LayerMask mask in layerMask.Values)
                    {
                        yield return CalculateTarget(rayValue, mask);
                    }
                }
            }
        }

        private Vector3 CalculateTarget(Ray rayValue, LayerMask mask)
        {
            Vector3 direction = rayValue.direction;
            if (direction.sqrMagnitude == 0f)
            {
                Debug.LogWarning($"{nameof(RayHitPosition)}: Ray direction has zero magnitude.");
                return rayValue.origin;
            }

            direction.Normalize();
            float rayDistance = Mathf.Max(0f, maxDistance);

            bool hasHit = Physics.Raycast(rayValue.origin, direction, out RaycastHit hit, rayDistance, mask);
            if (hasHit)
            {
                return hit.point;
            }

            float fallbackDistance = Mathf.Max(0f, distanceWhenNoHit);
            return rayValue.origin + direction * fallbackDistance;
        }
    }
}
