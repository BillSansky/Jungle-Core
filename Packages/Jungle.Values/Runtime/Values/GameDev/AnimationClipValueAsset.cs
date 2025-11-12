using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing an animation clip.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip value", fileName = "AnimationClipValue")]
    [JungleClassInfo("Animation Clip Value Asset", "ScriptableObject storing an animation clip.", null, "Game Dev")]
    public class AnimationClipValueAsset : ValueAsset<AnimationClip>
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
    /// ScriptableObject storing a list of animation clips.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip list value", fileName = "AnimationClipListValue")]
    [JungleClassInfo("Animation Clip List Asset", "ScriptableObject storing a list of animation clips.", null, "Game Dev")]
    public class AnimationClipListValueAsset : SerializedValueListAsset<AnimationClip>
    {
    }
    /// <summary>
    /// Reads an animation clip from an AnimationClipValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Animation Clip Value From Asset", "Reads an animation clip from an AnimationClipValueAsset.", null, "Game Dev")]
    public class AnimationClipValueFromAsset :
        ValueFromAsset<AnimationClip, AnimationClipValueAsset>, ISettableAnimationClipValue
    {
    }
    /// <summary>
    /// Reads animation clips from an AnimationClipListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Animation Clip List From Asset", "Reads animation clips from an AnimationClipListValueAsset.", null, "Game Dev")]
    public class AnimationClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationClip>, AnimationClipListValueAsset>
    {
    }
}
