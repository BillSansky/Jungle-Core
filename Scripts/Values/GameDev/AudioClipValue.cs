using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAudioClipValue : IValue<AudioClip>
    {
    }

    [Serializable]
    public class AudioClipValue : LocalValue<AudioClip>, IAudioClipValue
    {
        public override bool HasMultipleValues => false;
        
        
    }

    [Serializable]
    public class AudioClipClassMembersValue : ClassMembersValue<AudioClip>, IAudioClipValue
    {
    }
}
