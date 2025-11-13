using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Provides access to the current mouse position in screen coordinates.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Mouse Position", "Provides the current mouse position in screen coordinates.", null, "Values/Unity Primitives")]
    public class MousePositionValue : IVector3Value
    {
        /// <inheritdoc />
        public Vector3 Value()
        {
            return Input.mousePosition;
        }

        /// <inheritdoc />
        public bool HasMultipleValues => false;
    }
}
