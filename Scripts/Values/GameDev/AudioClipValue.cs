using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IAudioClipValue contract.
    /// </summary>
    public interface IAudioClipValue : IValue<AudioClip>
    {
    }
    /// <summary>
    /// Stores an AudioClip reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class AudioClipValue : LocalValue<AudioClip>, IAudioClipValue
    {
        public override bool HasMultipleValues => false;
        
        
    }
    /// <summary>
    /// Resolves an AudioClip reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class AudioClipClassMembersValue : ClassMembersValue<AudioClip>, IAudioClipValue
    {
    }
}
