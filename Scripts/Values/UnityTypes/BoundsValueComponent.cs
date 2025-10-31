using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Bounds Value Component", "Component exposing a Bounds value.", null, "Values/Unity Types")]
    public class BoundsValueComponent : ValueComponent<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);

        public override Bounds Value()
        {
            return value;
        }

        public override void SetValue(Bounds value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Bounds List Component", "Component exposing a list of Bounds values.", null, "Values/Unity Types")]
    public class BoundsListValueComponent : SerializedValueListComponent<Bounds>
    {
    }

    [Serializable]
    [JungleClassInfo("Bounds Value From Component", "Reads a Bounds value from a BoundsValueComponent.", null, "Values/Unity Types")]
    public class BoundsValueFromComponent : ValueFromComponent<Bounds, BoundsValueComponent>, IBoundsValue
    {
    }

    [Serializable]
    [JungleClassInfo("Bounds List From Component", "Reads Bounds values from a BoundsListValueComponent.", null, "Values/Unity Types")]
    public class BoundsListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Bounds>, BoundsListValueComponent>
    {
    }
}
