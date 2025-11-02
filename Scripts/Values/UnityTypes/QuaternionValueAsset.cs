using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a rotation quaternion.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion value", fileName = "QuaternionValue")]
    [JungleClassInfo("Quaternion Value Asset", "ScriptableObject storing a rotation quaternion.", null, "Values/Unity Types")]
    public class QuaternionValueAsset : ValueAsset<Quaternion>
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
    /// ScriptableObject storing a list of quaternions.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion list value", fileName = "QuaternionListValue")]
    [JungleClassInfo("Quaternion List Asset", "ScriptableObject storing a list of quaternions.", null, "Values/Unity Types")]
    public class QuaternionListValueAsset : SerializedValueListAsset<Quaternion>
    {
    }
    /// <summary>
    /// Reads a rotation quaternion from a QuaternionValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Quaternion Value From Asset", "Reads a rotation quaternion from a QuaternionValueAsset.", null, "Values/Unity Types")]
    public class QuaternionValueFromAsset : ValueFromAsset<Quaternion, QuaternionValueAsset>, IQuaternionValue
    {
    }
    /// <summary>
    /// Reads quaternions from a QuaternionListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Quaternion List From Asset", "Reads quaternions from a QuaternionListValueAsset.", null, "Values/Unity Types")]
    public class QuaternionListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Quaternion>, QuaternionListValueAsset>
    {
    }
}
