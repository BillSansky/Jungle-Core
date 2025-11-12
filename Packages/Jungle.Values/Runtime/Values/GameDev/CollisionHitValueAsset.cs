using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing raycast hit information.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collision hit value", fileName = "CollisionHitValueAsset")]
    [JungleClassInfo("Collision Hit Value Asset", "ScriptableObject storing raycast hit information.", null, "Game Dev")]
    public class CollisionHitValueAsset : ValueAsset<RaycastHit>
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
    /// ScriptableObject storing a list of raycast hits.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collision hit list value", fileName = "CollisionHitListValue")]
    [JungleClassInfo("Collision Hit List Asset", "ScriptableObject storing a list of raycast hits.", null, "Game Dev")]
    public class CollisionHitListValueAsset : SerializedValueListAsset<RaycastHit>
    {
    }

    /// <summary>
    /// Reads raycast hit information from a <see cref="CollisionHitValueAsset"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Value From Asset", "Reads raycast hit information from a CollisionHitValueAsset.", null, "Game Dev")]
    public class CollisionHitValueFromAsset : ValueFromAsset<RaycastHit, CollisionHitValueAsset>, ICollisionHitValue
    {
    }

    /// <summary>
    /// Reads raycast hit information from a <see cref="CollisionHitListValueAsset"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit List From Asset", "Reads raycast hit information from a CollisionHitListValueAsset.", null, "Game Dev")]
    public class CollisionHitListValueFromAsset : ValueFromAsset<IReadOnlyList<RaycastHit>, CollisionHitListValueAsset>
    {
    }
}
