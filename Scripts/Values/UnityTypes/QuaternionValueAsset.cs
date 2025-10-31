using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion value", fileName = "QuaternionValue")]
    [JungleClassInfo("Quaternion Value Asset", "ScriptableObject storing a rotation quaternion.", null, "Values/Unity Types")]
    public class QuaternionValueAsset : ValueAsset<Quaternion>
    {
        [SerializeField]
        private Quaternion value;

        public override Quaternion Value()
        {
            return value;
        }

        public override void SetValue(Quaternion value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion list value", fileName = "QuaternionListValue")]
    [JungleClassInfo("Quaternion List Asset", "ScriptableObject storing a list of quaternions.", null, "Values/Unity Types")]
    public class QuaternionListValueAsset : SerializedValueListAsset<Quaternion>
    {
    }

    [Serializable]
    [JungleClassInfo("Quaternion Value From Asset", "Reads a rotation quaternion from a QuaternionValueAsset.", null, "Values/Unity Types")]
    public class QuaternionValueFromAsset : ValueFromAsset<Quaternion, QuaternionValueAsset>, IQuaternionValue
    {
    }

    [Serializable]
    [JungleClassInfo("Quaternion List From Asset", "Reads quaternions from a QuaternionListValueAsset.", null, "Values/Unity Types")]
    public class QuaternionListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Quaternion>, QuaternionListValueAsset>
    {
    }
}
