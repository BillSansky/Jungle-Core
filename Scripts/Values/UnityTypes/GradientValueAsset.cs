using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient value", fileName = "GradientValue")]
    [JungleClassInfo("Gradient Value Asset", "ScriptableObject storing a gradient.", null, "Values/Unity Types")]
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
    [JungleClassInfo("Gradient List Asset", "ScriptableObject storing a list of gradients.", null, "Values/Unity Types")]
    public class GradientListValueAsset : SerializedValueListAsset<Gradient>
    {
    }

    [Serializable]
    [JungleClassInfo("Gradient Value From Asset", "Reads a gradient from a GradientValueAsset.", null, "Values/Unity Types")]
    public class GradientValueFromAsset : ValueFromAsset<Gradient, GradientValueAsset>, IGradientValue
    {
    }

    [Serializable]
    [JungleClassInfo("Gradient List From Asset", "Reads gradients from a GradientListValueAsset.", null, "Values/Unity Types")]
    public class GradientListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Gradient>, GradientListValueAsset>
    {
    }
}
