using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing an animation clip.
    /// </summary>
    [JungleClassInfo("Animation Clip Value Component", "Component exposing an animation clip.", null, "Game Dev")]
    public class AnimationClipValueComponent : ValueComponent<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override AnimationClip Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(AnimationClip value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of animation clips.
    /// </summary>

    [JungleClassInfo("Animation Clip List Component", "Component exposing a list of animation clips.", null, "Game Dev")]
    public class AnimationClipListValueComponent : SerializedValueListComponent<AnimationClip>
    {
    }
    /// <summary>
    /// Reads an animation clip from an AnimationClipValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Animation Clip Value From Component", "Reads an animation clip from an AnimationClipValueComponent.", null, "Game Dev")]
    public class AnimationClipValueFromComponent :
        ValueFromComponent<AnimationClip, AnimationClipValueComponent>, ISettableAnimationClipValue
    {
    }
    /// <summary>
    /// Reads animation clips from an AnimationClipListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Animation Clip List From Component", "Reads animation clips from an AnimationClipListValueComponent.", null, "Game Dev")]
    public class AnimationClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AnimationClip>, AnimationClipListValueComponent>
    {
    }
}
