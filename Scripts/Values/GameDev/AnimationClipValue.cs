using System;
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
}
