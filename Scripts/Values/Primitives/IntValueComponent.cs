using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [JungleClassInfo("Int Value Component", "Component exposing a integer number.", null, "Values/Primitives")]
    public class IntValueComponent : ValueComponent<int>
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

    [JungleClassInfo("Int List Component", "Component exposing a list of integer numbers.", null, "Values/Primitives")]
    public class IntListValueComponent : SerializedValueListComponent<int>
    {
    }

    [Serializable]
    [JungleClassInfo("Int Value From Component", "Reads a integer number from a IntValueComponent.", null, "Values/Primitives")]
    public class IntValueFromComponent : ValueFromComponent<int, IntValueComponent>, IIntValue
    {
    }

    [Serializable]
    [JungleClassInfo("Int List From Component", "Reads integer numbers from a IntListValueComponent.", null, "Values/Primitives")]
    public class IntListValueFromComponent : ValueFromComponent<IReadOnlyList<int>, IntListValueComponent>
    {
    }
}
