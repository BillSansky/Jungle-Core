using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip value", fileName = "AudioClipValue")]
    [JungleClassInfo("Audio Clip Value Asset", "ScriptableObject storing an audio clip.", null, "Values/Game Dev")]
    public class AudioClipValueAsset : ValueAsset<AudioClip>
    {
        [SerializeField]
        private AudioClip value;

        public override AudioClip Value()
        {
            return value;
        }

        public override void SetValue(AudioClip value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/AudioClip list value", fileName = "AudioClipListValue")]
    [JungleClassInfo("Audio Clip List Asset", "ScriptableObject storing a list of audio clips.", null, "Values/Game Dev")]
    public class AudioClipListValueAsset : SerializedValueListAsset<AudioClip>
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Clip Value From Asset", "Reads an audio clip from an AudioClipValueAsset.", null, "Values/Game Dev")]
    public class AudioClipValueFromAsset :
        ValueFromAsset<AudioClip, AudioClipValueAsset>, IAudioClipValue
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Clip List From Asset", "Reads audio clips from an AudioClipListValueAsset.", null, "Values/Game Dev")]
    public class AudioClipListValueFromAsset :
        ValueFromAsset<IReadOnlyList<AudioClip>, AudioClipListValueAsset>
    {
    }
}
