using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing an AnimationClip reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip value", fileName = "AnimationClipValue")]
    public class AnimationClipValueAsset : ValueAsset<AnimationClip>
    {
        [SerializeField]
        private AnimationClip value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override AnimationClip Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(AnimationClip value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of AnimationClip references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AnimationClip list value", fileName = "AnimationClipListValue")]
    public class AnimationClipListValueAsset : SerializedValueListAsset<AnimationClip>
    {
    }
    /// <summary>
    /// Value wrapper that reads an AnimationClip reference from an assigned AnimationClipValueAsset.
    /// </summary>
    [Serializable]
    public class AnimationClipValueFromAsset :
        ValueFromAsset<AnimationClip, AnimationClipValueAsset>, IAnimationClipValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of AnimationClip references from an assigned AnimationClipListValueAsset.
    /// </summary>
    [Serializable]
    public class AnimationClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AnimationClip>, AnimationClipListValueAsset>
    {
    }
}
