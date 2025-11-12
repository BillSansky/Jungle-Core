using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Component exposing a integer number.
    /// </summary>
    [JungleClassInfo("Int Value Component", "Component exposing a integer number.", null, "Primitives")]
    public class IntValueComponent : ValueComponent<int>
    {
        [SerializeField]
        private int value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override int Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of integer numbers.
    /// </summary>

    [JungleClassInfo("Int List Component", "Component exposing a list of integer numbers.", null, "Primitives")]
    public class IntListValueComponent : SerializedValueListComponent<int>
    {
    }
    /// <summary>
    /// Reads a integer number from a IntValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Int Value From Component", "Reads a integer number from a IntValueComponent.", null, "Primitives")]
    public class IntValueFromComponent : ValueFromComponent<int, IntValueComponent>, ISettableIntValue
    {
    }
    /// <summary>
    /// Reads integer numbers from a IntListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Int List From Component", "Reads integer numbers from a IntListValueComponent.", null, "Primitives")]
    public class IntListValueFromComponent : ValueFromComponent<IReadOnlyList<int>, IntListValueComponent>
    {
    }
}
