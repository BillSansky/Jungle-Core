using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool value", fileName = "BoolValue")]
    public class BoolValueAsset : ValueAsset<bool>
    {
        [SerializeField]
        private bool value;

        public override bool Value()
        {
            return value;
        }
    }

    [Serializable]
    public class BoolValueFromAsset : ValueFromAsset<bool, BoolValueAsset>, IBoolValue
    {
    }
}
