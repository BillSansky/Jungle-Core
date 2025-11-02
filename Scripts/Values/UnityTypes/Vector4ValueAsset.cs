using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 value", fileName = "Vector4Value")]
    [JungleClassInfo("Vector4 Value Asset", "ScriptableObject storing a 4D vector.", null, "Values/Unity Types")]
    public class Vector4ValueAsset : ValueAsset<Vector4>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 list value", fileName = "Vector4ListValue")]
    [JungleClassInfo("Vector4 List Asset", "ScriptableObject storing a list of 4D vectors.", null, "Values/Unity Types")]
    public class Vector4ListValueAsset : SerializedValueListAsset<Vector4>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector4 Value From Asset", "Reads a 4D vector from a Vector4ValueAsset.", null, "Values/Unity Types")]
    public class Vector4ValueFromAsset : ValueFromAsset<Vector4, Vector4ValueAsset>, IVector4Value
    {
    }

    [Serializable]
    [JungleClassInfo("Vector4 List From Asset", "Reads 4D vectors from a Vector4ListValueAsset.", null, "Values/Unity Types")]
    public class Vector4ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector4>, Vector4ListValueAsset>
    {
    }
}
