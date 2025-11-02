using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int value", fileName = "IntValue")]
    [JungleClassInfo("Int Value Asset", "ScriptableObject storing a integer number.", null, "Values/Primitives")]
    public class IntValueAsset : ValueAsset<int>
    {
        [SerializeField]
        private int value;

        public override int Value()
        {
            return value;
        }

        public override void SetValue(int value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int list value", fileName = "IntListValue")]
    [JungleClassInfo("Int List Asset", "ScriptableObject storing a list of integer numbers.", null, "Values/Primitives")]
    public class IntListValueAsset : SerializedValueListAsset<int>
    {
    }

    [Serializable]
    [JungleClassInfo("Int Value From Asset", "Reads a integer number from a IntValueAsset.", null, "Values/Primitives")]
    public class IntValueFromAsset : ValueFromAsset<int, IntValueAsset>, IIntValue
    {
    }

    [Serializable]
    [JungleClassInfo("Int List From Asset", "Reads integer numbers from a IntListValueAsset.", null, "Values/Primitives")]
    public class IntListValueFromAsset : ValueFromAsset<IReadOnlyList<int>, IntListValueAsset>
    {
    }
}
