using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool Value", fileName = "BoolValue")]
    public class BoolValueAsset : ValueAsset<bool>
    {
        [SerializeField]
        private bool value;

        public override bool GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class BoolValueFromAsset : ValueFromAsset<bool, BoolValueAsset>, IBoolValue
    {
    }
}
