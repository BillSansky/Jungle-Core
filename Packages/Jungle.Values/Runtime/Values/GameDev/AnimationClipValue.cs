using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns an AnimationClip reference.
    /// </summary>
    public interface IAnimationClipValue : IValue<AnimationClip>
    {
    }
    public interface ISettableAnimationClipValue : IAnimationClipValue, IValueSableValue<AnimationClip>
    {
    }
    /// <summary>
    /// Stores an animation clip directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Animation Clip Value", "Stores an animation clip directly on the owner.", null, "Values/Game Dev", true)]
    public class AnimationClipValue : LocalValue<AnimationClip>, ISettableAnimationClipValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns an animation clip from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Animation Clip Member Value", "Returns an animation clip from a component field, property, or method.", null, "Values/Game Dev")]
    public class AnimationClipClassMembersValue : ClassMembersValue<AnimationClip>, IAnimationClipValue
    {
    }
}
