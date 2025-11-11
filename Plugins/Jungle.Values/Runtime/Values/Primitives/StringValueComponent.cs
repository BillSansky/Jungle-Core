using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Component exposing a text string.
    /// </summary>
    [JungleClassInfo("String Value Component", "Component exposing a text string.", null, "Values/Primitives")]
    public class StringValueComponent : ValueComponent<string>
    {
        [SerializeField]
        private string value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override string Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(string value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of text strings.
    /// </summary>

    [JungleClassInfo("String List Component", "Component exposing a list of text strings.", null, "Values/Primitives")]
    public class StringListValueComponent : SerializedValueListComponent<string>
    {
    }
    /// <summary>
    /// Reads a text string from a StringValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("String Value From Component", "Reads a text string from a StringValueComponent.", null, "Values/Primitives")]
    public class StringValueFromComponent : ValueFromComponent<string, StringValueComponent>, IStringValue
    {
    }
    /// <summary>
    /// Reads text strings from a StringListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("String List From Component", "Reads text strings from a StringListValueComponent.", null, "Values/Primitives")]
    public class StringListValueFromComponent : ValueFromComponent<IReadOnlyList<string>, StringListValueComponent>
    {
    }
}
