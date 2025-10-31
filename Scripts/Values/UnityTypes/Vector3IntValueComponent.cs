using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Vector3Int Value Component", "Component exposing a 3D integer vector.", null, "Values/Unity Types")]
    public class Vector3IntValueComponent : ValueComponent<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;

        public override Vector3Int Value()
        {
            return value;
        }

        public override void SetValue(Vector3Int value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Vector3Int List Component", "Component exposing a list of 3D integer vectors.", null, "Values/Unity Types")]
    public class Vector3IntListValueComponent : SerializedValueListComponent<Vector3Int>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3Int Value From Component", "Reads a 3D integer vector from a Vector3IntValueComponent.", null, "Values/Unity Types")]
    public class Vector3IntValueFromComponent :
        ValueFromComponent<Vector3Int, Vector3IntValueComponent>, IVector3IntValue
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3Int List From Component", "Reads 3D integer vectors from a Vector3IntListValueComponent.", null, "Values/Unity Types")]
    public class Vector3IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3Int>, Vector3IntListValueComponent>
    {
    }
}
