using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAudioSourceValue : IValue<AudioSource>
    {
        
    }

    [Serializable]
    public class AudioSourceLocalValue : LocalValue<AudioSource>, IAudioSourceValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
