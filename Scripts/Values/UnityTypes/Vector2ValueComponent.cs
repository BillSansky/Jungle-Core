using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Vector2 Value Component", "Component exposing a 2D vector.", null, "Values/Unity Types")]
    public class Vector2ValueComponent : ValueComponent<Vector2>
    {
        [SerializeField]
        private Vector2 value;

        public override Vector2 Value()
        {
            return value;
        }

        public override void SetValue(Vector2 value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Vector2 List Component", "Component exposing a list of 2D vectors.", null, "Values/Unity Types")]
    public class Vector2ListValueComponent : SerializedValueListComponent<Vector2>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector2 Value From Component", "Reads a 2D vector from a Vector2ValueComponent.", null, "Values/Unity Types")]
    public class Vector2ValueFromComponent : ValueFromComponent<Vector2, Vector2ValueComponent>, IVector2Value
    {
    }

    [Serializable]
    [JungleClassInfo("Vector2 List From Component", "Reads 2D vectors from a Vector2ListValueComponent.", null, "Values/Unity Types")]
    public class Vector2ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2>, Vector2ListValueComponent>
    {
    }
}
