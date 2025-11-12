using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns an AudioClip reference.
    /// </summary>
    public interface IAudioClipValue : IValue<AudioClip>
    {
    }
    public interface ISettableAudioClipValue : IAudioClipValue, IValueSableValue<AudioClip>
    {
    }
    /// <summary>
    /// Stores an audio clip directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Clip Value", "Stores an audio clip directly on the owner.", null, "Game Dev", true)]
    public class AudioClipValue : LocalValue<AudioClip>, ISettableAudioClipValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
        
    }
    /// <summary>
    /// Returns an audio clip from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Audio Clip Member Value", "Returns an audio clip from a component field, property, or method.", null, "Game Dev")]
    public class AudioClipClassMembersValue : ClassMembersValue<AudioClip>, IAudioClipValue
    {
    }
}
