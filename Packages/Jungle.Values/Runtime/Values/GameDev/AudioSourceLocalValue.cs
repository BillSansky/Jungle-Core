using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to an AudioSource reference.
    /// </summary>
    public interface IAudioSourceValue : IValue<AudioSource>
    {

    }
    public interface ISettableAudioSourceValue : IAudioSourceValue, IValueSableValue<AudioSource>
    {
    }
    /// <summary>
    /// Stores an audio source component directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Source Value", "Stores an audio source component directly on the owner.", null, "Values/Game Dev", true)]
    public class AudioSourceLocalValue : LocalValue<AudioSource>, ISettableAudioSourceValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns an audio source component from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Source Member Value", "Returns an audio source component from a component field, property, or method.", null, "Values/Game Dev")]
    public class AudioSourceClassMembersValue : ClassMembersValue<AudioSource>, IAudioSourceValue
    {
       
    }
}
