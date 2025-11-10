using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a rotation quaternion.
    /// </summary>
    [JungleClassInfo("Quaternion Value Component", "Component exposing a rotation quaternion.", null, "Values/Unity Types")]
    public class QuaternionValueComponent : ValueComponent<Quaternion>
    {
        [SerializeField]
        private Quaternion value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Quaternion Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Quaternion value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of quaternions.
    /// </summary>

    [JungleClassInfo("Quaternion List Component", "Component exposing a list of quaternions.", null, "Values/Unity Types")]
    public class QuaternionListValueComponent : SerializedValueListComponent<Quaternion>
    {
    }
    /// <summary>
    /// Reads a rotation quaternion from a QuaternionValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Quaternion Value From Component", "Reads a rotation quaternion from a QuaternionValueComponent.", null, "Values/Unity Types")]
    public class QuaternionValueFromComponent : ValueFromComponent<Quaternion, QuaternionValueComponent>,
        IQuaternionValue
    {
    }
    /// <summary>
    /// Reads quaternions from a QuaternionListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Quaternion List From Component", "Reads quaternions from a QuaternionListValueComponent.", null, "Values/Unity Types")]
    public class QuaternionListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Quaternion>, QuaternionListValueComponent>
    {
    }
}
