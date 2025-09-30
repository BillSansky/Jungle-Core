using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IAudioClipValue : IValue<AudioClip>
    {
    }

    [Serializable]
    public class AudioClipValue : LocalValue<AudioClip>, IAudioClipValue
    {
    }
}
