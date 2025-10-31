using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes an AnimationCurve so scene objects can expose it to Jungle systems.
    /// </summary>
    public class AnimationCurveValueComponent : ValueComponent<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override AnimationCurve Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(AnimationCurve value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of AnimationCurve values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class AnimationCurveListValueComponent : SerializedValueListComponent<AnimationCurve>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AnimationCurve from a AnimationCurveValueComponent component.
    /// </summary>
    [Serializable]
    public class AnimationCurveValueFromComponent :
        ValueFromComponent<AnimationCurve, AnimationCurveValueComponent>, IAnimationCurveValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AnimationCurve values from a AnimationCurveListValueComponent component.
    /// </summary>
    [Serializable]
    public class AnimationCurveListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationCurve>, AnimationCurveListValueComponent>
    {
    }
}
