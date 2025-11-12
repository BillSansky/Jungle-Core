using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a Rect area.
    /// </summary>
    [JungleClassInfo("Rect Value Component", "Component exposing a Rect area.", null, "Unity Types")]
    public class RectValueComponent : ValueComponent<Rect>
    {
        [SerializeField]
        private Rect value = new Rect(0f, 0f, 1f, 1f);
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Rect Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Rect value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of rectangles.
    /// </summary>

    [JungleClassInfo("Rect List Component", "Component exposing a list of rectangles.", null, "Unity Types")]
    public class RectListValueComponent : SerializedValueListComponent<Rect>
    {
    }
    /// <summary>
    /// Reads a Rect area from a RectValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rect Value From Component", "Reads a Rect area from a RectValueComponent.", null, "Unity Types")]
    public class RectValueFromComponent : ValueFromComponent<Rect, RectValueComponent>, ISettableRectValue
    {
    }
    /// <summary>
    /// Reads rectangles from a RectListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rect List From Component", "Reads rectangles from a RectListValueComponent.", null, "Unity Types")]
    public class RectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rect>, RectListValueComponent>
    {
    }
}
