using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAudioSourceValue : IComponent<AudioSource>
    {
        
    }

    [Serializable]
    [JungleClassInfo("Audio Source Value", "Stores an audio source component directly on the owner.", null, "Values/Game Dev", true)]
    public class AudioSourceLocalValue : LocalValue<AudioSource>, IAudioSourceValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Audio Source Member Value", "Returns an audio source component from a component field, property, or method.", null, "Values/Game Dev")]
    public class AudioSourceClassMembersValue : ComponentClassMembersValue<AudioSource>, IAudioSourceValue
    {
    }
}
