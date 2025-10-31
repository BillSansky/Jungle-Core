using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioSource value", fileName = "AudioSourceLocalValue")]
    [JungleClassInfo("Audio Source Value Asset", "ScriptableObject storing an audio source component.", null, "Values/Game Dev")]
    public class AudioSourceValueAsset : ValueAsset<AudioSource>
    {
        [SerializeField]
        private AudioSource value;

        public override AudioSource Value()
        {
            return value;
        }

        public override void SetValue(AudioSource value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioSource list value", fileName = "AudioSourceListValue")]
    [JungleClassInfo("Audio Source List Asset", "ScriptableObject storing a list of audio sources.", null, "Values/Game Dev")]
    public class AudioSourceListValueAsset : SerializedValueListAsset<AudioSource>
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Source Value From Asset", "Reads an audio source component from an AudioSourceValueAsset.", null, "Values/Game Dev")]
    public class AudioSourceValueFromAsset :
        ValueFromAsset<AudioSource, AudioSourceValueAsset>, IAudioSourceValue
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Source List From Asset", "Reads audio sources from an AudioSourceListValueAsset.", null, "Values/Game Dev")]
    public class AudioSourceListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioSource>, AudioSourceListValueAsset>
    {
    }
}
