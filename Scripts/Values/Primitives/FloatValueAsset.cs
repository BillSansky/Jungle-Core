using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float value", fileName = "FloatValue")]
    public class FloatValueAsset : ValueAsset<float>
    {
        [SerializeField]
        private float value;

        public override float Value()
        {
            return value;
        }
    }

    [Serializable]
    public class FloatValueFromAsset : ValueFromAsset<float, FloatValueAsset>, IFloatValue
    {
    }
}
