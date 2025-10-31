using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Vector3 Value Component", "Component exposing a 3D vector.", null, "Values/Unity Types")]
    public class Vector3ValueComponent : ValueComponent<Vector3>
    {
        [SerializeField]
        private Vector3 value;

        public override Vector3 Value()
        {
            return value;
        }

        public override void SetValue(Vector3 value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Vector3 List Component", "Component exposing a list of 3D vectors.", null, "Values/Unity Types")]
    public class Vector3ListValueComponent : SerializedValueListComponent<Vector3>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3 Value From Component", "Reads a 3D vector from a Vector3ValueComponent.", null, "Values/Unity Types")]
    public class Vector3ValueFromComponent : ValueFromComponent<Vector3, Vector3ValueComponent>, IVector3Value
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3 List From Component", "Reads 3D vectors from a Vector3ListValueComponent.", null, "Values/Unity Types")]
    public class Vector3ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3>, Vector3ListValueComponent>
    {
    }
}
