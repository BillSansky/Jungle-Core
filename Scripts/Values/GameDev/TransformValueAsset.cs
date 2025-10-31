using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a Transform reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform value", fileName = "TransformLocalValue")]
    public class TransformValueAsset : ValueAsset<Transform>
    {
        [SerializeField]
        private Transform value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Transform Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Transform value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Transform references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform list value", fileName = "TransformListValue")]
    public class TransformListValueAsset : SerializedValueListAsset<Transform>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Transform reference from an assigned TransformValueAsset.
    /// </summary>
    [Serializable]
    public class TransformValueFromAsset :
        ValueFromAsset<Transform, TransformValueAsset>, ITransformValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Transform references from an assigned TransformListValueAsset.
    /// </summary>
    [Serializable]
    public class TransformListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Transform>, TransformListValueAsset>
    {
    }
}
