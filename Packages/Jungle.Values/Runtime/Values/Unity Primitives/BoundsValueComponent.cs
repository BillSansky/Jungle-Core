using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a Bounds value.
    /// </summary>
    [JungleClassInfo("Bounds Value Component", "Component exposing a Bounds value.", null, "Unity Types")]
    public class BoundsValueComponent : ValueComponent<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Bounds Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Bounds value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of Bounds values.
    /// </summary>

    [JungleClassInfo("Bounds List Component", "Component exposing a list of Bounds values.", null, "Unity Types")]
    public class BoundsListValueComponent : SerializedValueListComponent<Bounds>
    {
    }
    /// <summary>
    /// Reads a Bounds value from a BoundsValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bounds Value From Component", "Reads a Bounds value from a BoundsValueComponent.", null, "Unity Types")]
    public class BoundsValueFromComponent : ValueFromComponent<Bounds, BoundsValueComponent>, ISettableBoundsValue
    {
    }
    /// <summary>
    /// Reads Bounds values from a BoundsListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bounds List From Component", "Reads Bounds values from a BoundsListValueComponent.", null, "Unity Types")]
    public class BoundsListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Bounds>, BoundsListValueComponent>
    {
    }
}
