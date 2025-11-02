using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAudioClipValue : IValue<AudioClip>
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Clip Value", "Stores an audio clip directly on the owner.", null, "Values/Game Dev", true)]
    public class AudioClipValue : LocalValue<AudioClip>, IAudioClipValue
    {
        public override bool HasMultipleValues => false;
        
        
    }

    [Serializable]
    [JungleClassInfo("Audio Clip Member Value", "Returns an audio clip from a component field, property, or method.", null, "Values/Game Dev")]
    public class AudioClipClassMembersValue : ClassMembersValue<AudioClip>, IAudioClipValue
    {
    }
}
