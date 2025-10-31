using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing an AnimationCurve for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve value", fileName = "AnimationCurveValue")]
    public class AnimationCurveValueAsset : ValueAsset<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override AnimationCurve Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(AnimationCurve value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of AnimationCurve values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve list value", fileName = "AnimationCurveListValue")]
    public class AnimationCurveListValueAsset : SerializedValueListAsset<AnimationCurve>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AnimationCurve from an assigned AnimationCurveValueAsset.
    /// </summary>
    [Serializable]
    public class AnimationCurveValueFromAsset :
        ValueFromAsset<AnimationCurve, AnimationCurveValueAsset>, IAnimationCurveValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AnimationCurve values from an assigned AnimationCurveListValueAsset.
    /// </summary>
    [Serializable]
    public class AnimationCurveListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationCurve>, AnimationCurveListValueAsset>
    {
    }
}
