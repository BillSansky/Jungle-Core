using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAudioSourceValue : IComponent<AudioSource>
    {
        
    }

    [Serializable]
    public class AudioSourceLocalValue : LocalValue<AudioSource>, IAudioSourceValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class AudioSourceMethodInvokerValue : MethodInvokerValue<AudioSource>, IAudioSourceValue
    {
    }
}
