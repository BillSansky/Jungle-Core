using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a animation curve.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve value", fileName = "AnimationCurveValue")]
    [JungleClassInfo("AnimationCurve Value Asset", "ScriptableObject storing a animation curve.", null, "Unity Types")]
    public class AnimationCurveValueAsset : ValueAsset<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override AnimationCurve Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(AnimationCurve value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of animation curves.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve list value", fileName = "AnimationCurveListValue")]
    [JungleClassInfo("AnimationCurve List Asset", "ScriptableObject storing a list of animation curves.", null, "Unity Types")]
    public class AnimationCurveListValueAsset : SerializedValueListAsset<AnimationCurve>
    {
    }
    /// <summary>
    /// Reads a animation curve from a AnimationCurveValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("AnimationCurve Value From Asset", "Reads a animation curve from a AnimationCurveValueAsset.", null, "Unity Types")]
    public class AnimationCurveValueFromAsset :
        ValueFromAsset<AnimationCurve, AnimationCurveValueAsset>, ISettableAnimationCurveValue
    {
    }
    /// <summary>
    /// Reads animation curves from a AnimationCurveListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("AnimationCurve List From Asset", "Reads animation curves from a AnimationCurveListValueAsset.", null, "Unity Types")]
    public class AnimationCurveListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationCurve>, AnimationCurveListValueAsset>
    {
    }
}
