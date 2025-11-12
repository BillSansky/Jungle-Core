using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Component exposing a boolean flag.
    /// </summary>
    [JungleClassInfo("Bool Value Component", "Component exposing a boolean flag.", null, "Values/Primitives")]
    public class BoolValueComponent : ValueComponent<bool>
    {
        [SerializeField]
        private bool value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override bool Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of boolean flags.
    /// </summary>

    [JungleClassInfo("Bool List Component", "Component exposing a list of boolean flags.", null, "Values/Primitives")]
    public class BoolListValueComponent : SerializedValueListComponent<bool>
    {
    }
    /// <summary>
    /// Reads a boolean flag from a BoolValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bool Value From Component", "Reads a boolean flag from a BoolValueComponent.", null, "Values/Primitives")]
    public class BoolValueFromComponent : ValueFromComponent<bool, BoolValueComponent>, ISettableBoolValue
    {
    }
    /// <summary>
    /// Reads boolean flags from a BoolListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bool List From Component", "Reads boolean flags from a BoolListValueComponent.", null, "Values/Primitives")]
    public class BoolListValueFromComponent : ValueFromComponent<IReadOnlyList<bool>, BoolListValueComponent>
    {
    }
}
