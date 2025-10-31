using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Vector4 Value Component", "Component exposing a 4D vector.", null, "Values/Unity Types")]
    public class Vector4ValueComponent : ValueComponent<Vector4>
    {
        [SerializeField]
        private Vector4 value;

        public override Vector4 Value()
        {
            return value;
        }

        public override void SetValue(Vector4 value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Vector4 List Component", "Component exposing a list of 4D vectors.", null, "Values/Unity Types")]
    public class Vector4ListValueComponent : SerializedValueListComponent<Vector4>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector4 Value From Component", "Reads a 4D vector from a Vector4ValueComponent.", null, "Values/Unity Types")]
    public class Vector4ValueFromComponent : ValueFromComponent<Vector4, Vector4ValueComponent>, IVector4Value
    {
    }

    [Serializable]
    [JungleClassInfo("Vector4 List From Component", "Reads 4D vectors from a Vector4ListValueComponent.", null, "Values/Unity Types")]
    public class Vector4ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector4>, Vector4ListValueComponent>
    {
    }
}
