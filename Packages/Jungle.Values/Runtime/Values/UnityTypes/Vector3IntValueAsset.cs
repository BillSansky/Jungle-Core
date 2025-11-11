using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a 3D integer vector.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int value", fileName = "Vector3IntValue")]
    [JungleClassInfo("Vector3Int Value Asset", "ScriptableObject storing a 3D integer vector.", null, "Values/Unity Types")]
    public class Vector3IntValueAsset : ValueAsset<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Vector3Int Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Vector3Int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of 3D integer vectors.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int list value", fileName = "Vector3IntListValue")]
    [JungleClassInfo("Vector3Int List Asset", "ScriptableObject storing a list of 3D integer vectors.", null, "Values/Unity Types")]
    public class Vector3IntListValueAsset : SerializedValueListAsset<Vector3Int>
    {
    }
    /// <summary>
    /// Reads a 3D integer vector from a Vector3IntValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3Int Value From Asset", "Reads a 3D integer vector from a Vector3IntValueAsset.", null, "Values/Unity Types")]
    public class Vector3IntValueFromAsset :
        ValueFromAsset<Vector3Int, Vector3IntValueAsset>, IVector3IntValue
    {
    }
    /// <summary>
    /// Reads 3D integer vectors from a Vector3IntListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3Int List From Asset", "Reads 3D integer vectors from a Vector3IntListValueAsset.", null, "Values/Unity Types")]
    public class Vector3IntListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector3Int>, Vector3IntListValueAsset>
    {
    }
}
