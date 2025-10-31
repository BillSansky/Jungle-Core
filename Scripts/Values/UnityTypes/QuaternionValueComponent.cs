using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Quaternion Value Component", "Component exposing a rotation quaternion.", null, "Values/Unity Types")]
    public class QuaternionValueComponent : ValueComponent<Quaternion>
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

    [JungleClassInfo("Quaternion List Component", "Component exposing a list of quaternions.", null, "Values/Unity Types")]
    public class QuaternionListValueComponent : SerializedValueListComponent<Quaternion>
    {
    }

    [Serializable]
    [JungleClassInfo("Quaternion Value From Component", "Reads a rotation quaternion from a QuaternionValueComponent.", null, "Values/Unity Types")]
    public class QuaternionValueFromComponent : ValueFromComponent<Quaternion, QuaternionValueComponent>,
        IQuaternionValue
    {
    }

    [Serializable]
    [JungleClassInfo("Quaternion List From Component", "Reads quaternions from a QuaternionListValueComponent.", null, "Values/Unity Types")]
    public class QuaternionListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Quaternion>, QuaternionListValueComponent>
    {
    }
}
