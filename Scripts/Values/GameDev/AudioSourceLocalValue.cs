using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IAudioSourceValue contract.
    /// </summary>
    public interface IAudioSourceValue : IComponent<AudioSource>
    {
        
    }
    /// <summary>
    /// Stores an AudioSource reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class AudioSourceLocalValue : LocalValue<AudioSource>, IAudioSourceValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves an AudioSource reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class AudioSourceClassMembersValue : ClassMembersValue<AudioSource>, IAudioSourceValue
    {
    }
}
