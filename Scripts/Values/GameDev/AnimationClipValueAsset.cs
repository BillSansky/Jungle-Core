using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip value", fileName = "AnimationClipValue")]
    [JungleClassInfo("Animation Clip Value Asset", "ScriptableObject storing an animation clip.", null, "Values/Game Dev")]
    public class AnimationClipValueAsset : ValueAsset<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;

        public override AnimationClip Value()
        {
            return value;
        }

        public override void SetValue(AnimationClip value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip list value", fileName = "AnimationClipListValue")]
    [JungleClassInfo("Animation Clip List Asset", "ScriptableObject storing a list of animation clips.", null, "Values/Game Dev")]
    public class AnimationClipListValueAsset : SerializedValueListAsset<AnimationClip>
    {
    }

    [Serializable]
    [JungleClassInfo("Animation Clip Value From Asset", "Reads an animation clip from an AnimationClipValueAsset.", null, "Values/Game Dev")]
    public class AnimationClipValueFromAsset :
        ValueFromAsset<AnimationClip, AnimationClipValueAsset>, IAnimationClipValue
    {
    }

    [Serializable]
    [JungleClassInfo("Animation Clip List From Asset", "Reads animation clips from an AnimationClipListValueAsset.", null, "Values/Game Dev")]
    public class AnimationClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationClip>, AnimationClipListValueAsset>
    {
    }
}
