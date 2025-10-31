using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve value", fileName = "AnimationCurveValue")]
    [JungleClassInfo("AnimationCurve Value Asset", "ScriptableObject storing a animation curve.", null, "Values/Unity Types")]
    public class AnimationCurveValueAsset : ValueAsset<AnimationCurve>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/AnimationCurve list value", fileName = "AnimationCurveListValue")]
    [JungleClassInfo("AnimationCurve List Asset", "ScriptableObject storing a list of animation curves.", null, "Values/Unity Types")]
    public class AnimationCurveListValueAsset : SerializedValueListAsset<AnimationCurve>
    {
    }

    [Serializable]
    [JungleClassInfo("AnimationCurve Value From Asset", "Reads a animation curve from a AnimationCurveValueAsset.", null, "Values/Unity Types")]
    public class AnimationCurveValueFromAsset :
        ValueFromAsset<AnimationCurve, AnimationCurveValueAsset>, IAnimationCurveValue
    {
    }

    [Serializable]
    [JungleClassInfo("AnimationCurve List From Asset", "Reads animation curves from a AnimationCurveListValueAsset.", null, "Values/Unity Types")]
    public class AnimationCurveListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationCurve>, AnimationCurveListValueAsset>
    {
    }
}
