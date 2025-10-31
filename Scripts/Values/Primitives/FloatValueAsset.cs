using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float value", fileName = "FloatValue")]
    [JungleClassInfo("Float Value Asset", "ScriptableObject storing a floating-point number.", null, "Values/Primitives")]
    public class FloatValueAsset : ValueAsset<float>
    {
        [SerializeField]
        private float value;

        public override float Value()
        {
            return value;
        }

        public override void SetValue(float value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float list value", fileName = "FloatListValue")]
    [JungleClassInfo("Float List Asset", "ScriptableObject storing a list of floating-point numbers.", null, "Values/Primitives")]
    public class FloatListValueAsset : SerializedValueListAsset<float>
    {
    }

    [Serializable]
    [JungleClassInfo("Float Value From Asset", "Reads a floating-point number from a FloatValueAsset.", null, "Values/Primitives")]
    public class FloatValueFromAsset : ValueFromAsset<float, FloatValueAsset>, IFloatValue
    {
    }

    [Serializable]
    [JungleClassInfo("Float List From Asset", "Reads floating-point numbers from a FloatListValueAsset.", null, "Values/Primitives")]
    public class FloatListValueFromAsset : ValueFromAsset<IReadOnlyList<float>, FloatListValueAsset>
    {
    }
}
