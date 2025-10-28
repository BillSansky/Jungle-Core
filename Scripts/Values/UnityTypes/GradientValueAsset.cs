using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient value", fileName = "GradientValue")]
    public class GradientValueAsset : ValueAsset<Gradient>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient list value", fileName = "GradientListValue")]
    public class GradientListValueAsset : SerializedValueListAsset<Gradient>
    {
    }

    [Serializable]
    public class GradientValueFromAsset : ValueFromAsset<Gradient, GradientValueAsset>, IGradientValue
    {
    }

    [Serializable]
    public class GradientListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Gradient>, GradientListValueAsset>
    {
    }
}
