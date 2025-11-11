using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Reads the hit position from a collision hit value.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Position", "Reads the hit position from a collision hit value.", null, "Values/Unity Types")]
    public class CollisionHitPositionValue : IVector3Value
    {
        [SerializeReference]
        [JungleClassSelection]
        public ICollisionHitValue collisionHit = new CollisionHitLocalValue();

        /// <inheritdoc />
        public Vector3 Value()
        {
            return collisionHit.Value().point;
        }

        /// <inheritdoc />
        public bool HasMultipleValues => collisionHit.HasMultipleValues;

        /// <inheritdoc />
        public IEnumerable<Vector3> Values
        {
            get
            {
                foreach (RaycastHit hit in collisionHit.Values)
                {
                    yield return hit.point;
                }
            }
        }
    }

    /// <summary>
    /// Reads the hit normal from a collision hit value.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Normal", "Reads the hit normal from a collision hit value.", null, "Values/Unity Types")]
    public class CollisionHitNormalValue : IVector3Value
    {
        [SerializeReference]
        [JungleClassSelection]
        public ICollisionHitValue collisionHit = new CollisionHitLocalValue();

        /// <inheritdoc />
        public Vector3 Value()
        {
            return collisionHit.Value().normal;
        }

        /// <inheritdoc />
        public bool HasMultipleValues => collisionHit.HasMultipleValues;

        /// <inheritdoc />
        public IEnumerable<Vector3> Values
        {
            get
            {
                foreach (RaycastHit hit in collisionHit.Values)
                {
                    yield return hit.normal;
                }
            }
        }
    }
}
