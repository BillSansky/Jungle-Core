using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Audio Clip Value Component", "Component exposing an audio clip.", null, "Values/Game Dev")]
    public class AudioClipValueComponent : ValueComponent<AudioClip>
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

    [JungleClassInfo("Audio Clip List Component", "Component exposing a list of audio clips.", null, "Values/Game Dev")]
    public class AudioClipListValueComponent : SerializedValueListComponent<AudioClip>
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Clip Value From Component", "Reads an audio clip from an AudioClipValueComponent.", null, "Values/Game Dev")]
    public class AudioClipValueFromComponent :
        ValueFromComponent<AudioClip, AudioClipValueComponent>, IAudioClipValue
    {
    }

    [Serializable]
    [JungleClassInfo("Audio Clip List From Component", "Reads audio clips from an AudioClipListValueComponent.", null, "Values/Game Dev")]
    public class AudioClipListValueFromComponent :
        ValueFromComponent<IReadOnlyList<AudioClip>, AudioClipListValueComponent>
    {
    }
}
