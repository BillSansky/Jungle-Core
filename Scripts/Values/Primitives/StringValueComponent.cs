using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [JungleClassInfo("String Value Component", "Component exposing a text string.", null, "Values/Primitives")]
    public class StringValueComponent : ValueComponent<string>
    {
        [SerializeField]
        private string value;

        public override string Value()
        {
            return value;
        }

        public override void SetValue(string value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("String List Component", "Component exposing a list of text strings.", null, "Values/Primitives")]
    public class StringListValueComponent : SerializedValueListComponent<string>
    {
    }

    [Serializable]
    [JungleClassInfo("String Value From Component", "Reads a text string from a StringValueComponent.", null, "Values/Primitives")]
    public class StringValueFromComponent : ValueFromComponent<string, StringValueComponent>, IStringValue
    {
    }

    [Serializable]
    [JungleClassInfo("String List From Component", "Reads text strings from a StringListValueComponent.", null, "Values/Primitives")]
    public class StringListValueFromComponent : ValueFromComponent<IReadOnlyList<string>, StringListValueComponent>
    {
    }
}
