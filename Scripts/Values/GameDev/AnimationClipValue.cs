using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAnimationClipValue : IValue<AnimationClip>
    {
    }

    [Serializable]
    [JungleClassInfo("Animation Clip Value", "Stores an animation clip directly on the owner.", null, "Values/Game Dev", true)]
    public class AnimationClipValue : LocalValue<AnimationClip>, IAnimationClipValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Animation Clip Member Value", "Returns an animation clip from a component field, property, or method.", null, "Values/Game Dev")]
    public class AnimationClipClassMembersValue : ClassMembersValue<AnimationClip>, IAnimationClipValue
    {
    }
}
