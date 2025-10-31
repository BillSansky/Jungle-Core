using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 value", fileName = "Vector3Value")]
    [JungleClassInfo("Vector3 Value Asset", "ScriptableObject storing a 3D vector.", null, "Values/Unity Types")]
    public class Vector3ValueAsset : ValueAsset<Vector3>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 list value", fileName = "Vector3ListValue")]
    [JungleClassInfo("Vector3 List Asset", "ScriptableObject storing a list of 3D vectors.", null, "Values/Unity Types")]
    public class Vector3ListValueAsset : SerializedValueListAsset<Vector3>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3 Value From Asset", "Reads a 3D vector from a Vector3ValueAsset.", null, "Values/Unity Types")]
    public class Vector3ValueFromAsset : ValueFromAsset<Vector3, Vector3ValueAsset>, IVector3Value
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3 List From Asset", "Reads 3D vectors from a Vector3ListValueAsset.", null, "Values/Unity Types")]
    public class Vector3ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector3>, Vector3ListValueAsset>
    {
    }
}
