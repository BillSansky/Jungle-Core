using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Animation Clip Value Component", "Component exposing an animation clip.", null, "Values/Game Dev")]
    public class AnimationClipValueComponent : ValueComponent<AnimationClip>
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

    [JungleClassInfo("Animation Clip List Component", "Component exposing a list of animation clips.", null, "Values/Game Dev")]
    public class AnimationClipListValueComponent : SerializedValueListComponent<AnimationClip>
    {
    }

    [Serializable]
    [JungleClassInfo("Animation Clip Value From Component", "Reads an animation clip from an AnimationClipValueComponent.", null, "Values/Game Dev")]
    public class AnimationClipValueFromComponent :
        ValueFromComponent<AnimationClip, AnimationClipValueComponent>, IAnimationClipValue
    {
    }

    [Serializable]
    [JungleClassInfo("Animation Clip List From Component", "Reads animation clips from an AnimationClipListValueComponent.", null, "Values/Game Dev")]
    public class AnimationClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationClip>, AnimationClipListValueComponent>
    {
    }
}
