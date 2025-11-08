using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a transform component.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform value", fileName = "TransformLocalValue")]
    [JungleClassInfo("Transform Value Asset", "ScriptableObject storing a transform component.", null, "Values/Game Dev")]
    public class TransformValueAsset : ValueAsset<Transform>
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
    /// ScriptableObject storing a list of transforms.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform list value", fileName = "TransformListValue")]
    [JungleClassInfo("Transform List Asset", "ScriptableObject storing a list of transforms.", null, "Values/Game Dev")]
    public class TransformListValueAsset : SerializedValueListAsset<Transform>
    {
    }
    /// <summary>
    /// Reads a transform component from a TransformValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Transform Value From Asset", "Reads a transform component from a TransformValueAsset.", null, "Values/Game Dev")]
    public class TransformValueFromAsset :
        ValueFromAsset<Transform, TransformValueAsset>, ITransformValue
    {
    }
    /// <summary>
    /// Reads transforms from a TransformListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Transform List From Asset", "Reads transforms from a TransformListValueAsset.", null, "Values/Game Dev")]
    public class TransformListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Transform>, TransformListValueAsset>
    {
    }
}
