using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("AnimationCurve Value Component", "Component exposing a animation curve.", null, "Values/Unity Types")]
    public class AnimationCurveValueComponent : ValueComponent<AnimationCurve>
    {
        [SerializeField]
        private AnimationCurve value = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public override AnimationCurve Value()
        {
            return value;
        }

        public override void SetValue(AnimationCurve value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("AnimationCurve List Component", "Component exposing a list of animation curves.", null, "Values/Unity Types")]
    public class AnimationCurveListValueComponent : SerializedValueListComponent<AnimationCurve>
    {
    }

    [Serializable]
    [JungleClassInfo("AnimationCurve Value From Component", "Reads a animation curve from a AnimationCurveValueComponent.", null, "Values/Unity Types")]
    public class AnimationCurveValueFromComponent :
        ValueFromComponent<AnimationCurve, AnimationCurveValueComponent>, IAnimationCurveValue
    {
    }

    [Serializable]
    [JungleClassInfo("AnimationCurve List From Component", "Reads animation curves from a AnimationCurveListValueComponent.", null, "Values/Unity Types")]
    public class AnimationCurveListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationCurve>, AnimationCurveListValueComponent>
    {
    }
}
