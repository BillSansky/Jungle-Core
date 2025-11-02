using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 value", fileName = "Vector2Value")]
    [JungleClassInfo("Vector2 Value Asset", "ScriptableObject storing a 2D vector.", null, "Values/Unity Types")]
    public class Vector2ValueAsset : ValueAsset<Vector2>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 list value", fileName = "Vector2ListValue")]
    [JungleClassInfo("Vector2 List Asset", "ScriptableObject storing a list of 2D vectors.", null, "Values/Unity Types")]
    public class Vector2ListValueAsset : SerializedValueListAsset<Vector2>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector2 Value From Asset", "Reads a 2D vector from a Vector2ValueAsset.", null, "Values/Unity Types")]
    public class Vector2ValueFromAsset : ValueFromAsset<Vector2, Vector2ValueAsset>, IVector2Value
    {
    }

    [Serializable]
    [JungleClassInfo("Vector2 List From Asset", "Reads 2D vectors from a Vector2ListValueAsset.", null, "Values/Unity Types")]
    public class Vector2ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2>, Vector2ListValueAsset>
    {
    }
}
