using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [JungleClassInfo("Bool Value Component", "Component exposing a boolean flag.", null, "Values/Primitives")]
    public class BoolValueComponent : ValueComponent<bool>
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

    [JungleClassInfo("Bool List Component", "Component exposing a list of boolean flags.", null, "Values/Primitives")]
    public class BoolListValueComponent : SerializedValueListComponent<bool>
    {
    }

    [Serializable]
    [JungleClassInfo("Bool Value From Component", "Reads a boolean flag from a BoolValueComponent.", null, "Values/Primitives")]
    public class BoolValueFromComponent : ValueFromComponent<bool, BoolValueComponent>, IBoolValue
    {
    }

    [Serializable]
    [JungleClassInfo("Bool List From Component", "Reads boolean flags from a BoolListValueComponent.", null, "Values/Primitives")]
    public class BoolListValueFromComponent : ValueFromComponent<IReadOnlyList<bool>, BoolListValueComponent>
    {
    }
}
