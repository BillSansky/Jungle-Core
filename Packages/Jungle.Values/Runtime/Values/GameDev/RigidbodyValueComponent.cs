using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a rigidbody component.
    /// </summary>
    [JungleClassInfo("Rigidbody Value Component", "Component exposing a rigidbody component.", null, "Values/Game Dev")]
    public class RigidbodyValueComponent : ValueComponent<Rigidbody>
    {
        [SerializeField]
        private Rigidbody value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Rigidbody Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Rigidbody value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of rigidbodies.
    /// </summary>

    [JungleClassInfo("Rigidbody List Component", "Component exposing a list of rigidbodies.", null, "Values/Game Dev")]
    public class RigidbodyListValueComponent : SerializedValueListComponent<Rigidbody>
    {
    }
    /// <summary>
    /// Reads a rigidbody component from a RigidbodyValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rigidbody Value From Component", "Reads a rigidbody component from a RigidbodyValueComponent.", null, "Values/Game Dev")]
    public class RigidbodyValueFromComponent :
        ValueFromComponent<Rigidbody, RigidbodyValueComponent>, IRigidbodyValue
    {
    }
    /// <summary>
    /// Reads rigidbodies from a RigidbodyListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rigidbody List From Component", "Reads rigidbodies from a RigidbodyListValueComponent.", null, "Values/Game Dev")]
    public class RigidbodyListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rigidbody>, RigidbodyListValueComponent>
    {
    }
}
