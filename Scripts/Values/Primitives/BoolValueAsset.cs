using System;
using System.Collections.Generic;
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

        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool list value", fileName = "BoolListValue")]
    public class BoolListValueAsset : SerializedValueListAsset<bool>
    {
    }

    [Serializable]
    public class BoolValueFromAsset : ValueFromAsset<bool, BoolValueAsset>, IBoolValue
    {
    }

    [Serializable]
    public class BoolListValueFromAsset : ValueFromAsset<IReadOnlyList<bool>, BoolListValueAsset>
    {
    }
}
