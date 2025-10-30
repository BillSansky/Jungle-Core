using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAnimationClipValue : IValue<AnimationClip>
    {
    }

    [Serializable]
    public class AnimationClipValue : LocalValue<AnimationClip>, IAnimationClipValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class AnimationClipClassMembersValue : ClassMembersValue<AnimationClip>, IAnimationClipValue
    {
    }
}
