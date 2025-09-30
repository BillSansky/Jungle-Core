using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float Value", fileName = "FloatValue")]
    public class FloatValueAsset : ValueAsset<float>
    {
        [SerializeField]
        private float value;

        public override float GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class FloatValueFromAsset : ValueFromAsset<float, FloatValueAsset>, IFloatValue
    {
    }
}
