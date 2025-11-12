using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a rigidbody component.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody value", fileName = "RigidbodyValue")]
    [JungleClassInfo("Rigidbody Value Asset", "ScriptableObject storing a rigidbody component.", null, "Game Dev")]
    public class RigidbodyValueAsset : ValueAsset<Rigidbody>
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
    /// ScriptableObject storing a list of rigidbodies.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody list value", fileName = "RigidbodyListValue")]
    [JungleClassInfo("Rigidbody List Asset", "ScriptableObject storing a list of rigidbodies.", null, "Game Dev")]
    public class RigidbodyListValueAsset : SerializedValueListAsset<Rigidbody>
    {
    }
    /// <summary>
    /// Reads a rigidbody component from a RigidbodyValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rigidbody Value From Asset", "Reads a rigidbody component from a RigidbodyValueAsset.", null, "Game Dev")]
    public class RigidbodyValueFromAsset :
        ValueFromAsset<Rigidbody, RigidbodyValueAsset>, ISettableRigidbodyValue
    {
    }
    /// <summary>
    /// Reads rigidbodies from a RigidbodyListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rigidbody List From Asset", "Reads rigidbodies from a RigidbodyListValueAsset.", null, "Game Dev")]
    public class RigidbodyListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rigidbody>, RigidbodyListValueAsset>
    {
    }
}
