using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class GradientValueComponent : ValueComponent<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();

        public override Gradient Value()
        {
            return value;
        }

        public override void SetValue(Gradient value)
        {
            this.value = value;
        }
    }

    public class GradientListValueComponent : SerializedValueListComponent<Gradient>
    {
    }

    [Serializable]
    public class GradientValueFromComponent : ValueFromComponent<Gradient, GradientValueComponent>, IGradientValue
    {
    }

    [Serializable]
    public class GradientListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Gradient>, GradientListValueComponent>
    {
    }
}
