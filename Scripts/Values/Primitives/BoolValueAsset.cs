using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool value", fileName = "BoolValue")]
    [JungleClassInfo("Bool Value Asset", "ScriptableObject storing a boolean flag.", null, "Values/Primitives")]
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
    [JungleClassInfo("Bool List Asset", "ScriptableObject storing a list of boolean flags.", null, "Values/Primitives")]
    public class BoolListValueAsset : SerializedValueListAsset<bool>
    {
    }

    [Serializable]
    [JungleClassInfo("Bool Value From Asset", "Reads a boolean flag from a BoolValueAsset.", null, "Values/Primitives")]
    public class BoolValueFromAsset : ValueFromAsset<bool, BoolValueAsset>, IBoolValue
    {
    }

    [Serializable]
    [JungleClassInfo("Bool List From Asset", "Reads boolean flags from a BoolListValueAsset.", null, "Values/Primitives")]
    public class BoolListValueFromAsset : ValueFromAsset<IReadOnlyList<bool>, BoolListValueAsset>
    {
    }
}
