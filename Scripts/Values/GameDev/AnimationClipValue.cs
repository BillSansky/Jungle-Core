using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IAnimationClipValue contract.
    /// </summary>
    public interface IAnimationClipValue : IValue<AnimationClip>
    {
    }
    /// <summary>
    /// Stores an AnimationClip reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class AnimationClipValue : LocalValue<AnimationClip>, IAnimationClipValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves an AnimationClip reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class AnimationClipClassMembersValue : ClassMembersValue<AnimationClip>, IAnimationClipValue
    {
    }
}
