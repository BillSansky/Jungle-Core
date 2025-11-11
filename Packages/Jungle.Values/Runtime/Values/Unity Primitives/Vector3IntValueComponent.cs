using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a 3D integer vector.
    /// </summary>
    [JungleClassInfo("Vector3Int Value Component", "Component exposing a 3D integer vector.", null, "Values/Unity Types")]
    public class Vector3IntValueComponent : ValueComponent<Vector3Int>
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
    /// Component exposing a list of 3D integer vectors.
    /// </summary>

    [JungleClassInfo("Vector3Int List Component", "Component exposing a list of 3D integer vectors.", null, "Values/Unity Types")]
    public class Vector3IntListValueComponent : SerializedValueListComponent<Vector3Int>
    {
    }
    /// <summary>
    /// Reads a 3D integer vector from a Vector3IntValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3Int Value From Component", "Reads a 3D integer vector from a Vector3IntValueComponent.", null, "Values/Unity Types")]
    public class Vector3IntValueFromComponent :
        ValueFromComponent<Vector3Int, Vector3IntValueComponent>, ISettableVector3IntValue
    {
    }
    /// <summary>
    /// Reads 3D integer vectors from a Vector3IntListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3Int List From Component", "Reads 3D integer vectors from a Vector3IntListValueComponent.", null, "Values/Unity Types")]
    public class Vector3IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3Int>, Vector3IntListValueComponent>
    {
    }
}
