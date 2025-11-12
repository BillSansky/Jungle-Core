using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing raycast hit information.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Value Component", "Component exposing raycast hit information.", null, "Values/Game Dev")]
    public class CollisionHitValueComponent : ValueComponent<RaycastHit>
    {
        [SerializeField]
        private RaycastHit value;

        /// <inheritdoc />
        public override RaycastHit Value()
        {
            return value;
        }

        /// <inheritdoc />
        public override void SetValue(RaycastHit value)
        {
            this.value = value;
        }
    }

    /// <summary>
    /// Component exposing a list of raycast hits.
    /// </summary>
    [JungleClassInfo("Collision Hit List Component", "Component exposing a list of raycast hits.", null, "Values/Game Dev")]
    public class CollisionHitListValueComponent : SerializedValueListComponent<RaycastHit>
    {
    }

    /// <summary>
    /// Reads raycast hit information from a <see cref="CollisionHitValueComponent"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Value From Component", "Reads raycast hit information from a CollisionHitValueComponent.", null, "Values/Game Dev")]
    public class CollisionHitValueFromComponent : ValueFromComponent<RaycastHit, CollisionHitValueComponent>, ICollisionHitValue
    {
    }

    /// <summary>
    /// Reads raycast hits from a <see cref="CollisionHitListValueComponent"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit List From Component", "Reads raycast hits from a CollisionHitListValueComponent.", null, "Values/Game Dev")]
    public class CollisionHitListValueFromComponent : ValueFromComponent<IReadOnlyList<RaycastHit>, CollisionHitListValueComponent>
    {
    }
}
