using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
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

    public class BoundsListValueComponent : SerializedValueListComponent<Bounds>
    {
    }

    [Serializable]
    public class BoundsValueFromComponent : ValueFromComponent<Bounds, BoundsValueComponent>, IBoundsValue
    {
    }

    [Serializable]
    public class BoundsListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Bounds>, BoundsListValueComponent>
    {
    }
}
