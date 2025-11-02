using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a transform component.
    /// </summary>
    [JungleClassInfo("Transform Value Component", "Component exposing a transform component.", null, "Values/Game Dev")]
    public class TransformValueComponent : ValueComponent<Transform>
    {
        [SerializeField]
        private Transform value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Transform Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Transform value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of transforms.
    /// </summary>

    [JungleClassInfo("Transform List Component", "Component exposing a list of transforms.", null, "Values/Game Dev")]
    public class TransformListValueComponent : SerializedValueListComponent<Transform>
    {
    }
    /// <summary>
    /// Reads a transform component from a TransformValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Transform Value From Component", "Reads a transform component from a TransformValueComponent.", null, "Values/Game Dev")]
    public class TransformValueFromComponent :
        ValueFromComponent<Transform, TransformValueComponent>, ITransformValue
    {
    }
    /// <summary>
    /// Reads transforms from a TransformListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Transform List From Component", "Reads transforms from a TransformListValueComponent.", null, "Values/Game Dev")]
    public class TransformListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Transform>, TransformListValueComponent>
    {
    }
}
