using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves an AudioSource component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Audio Source Lookup Value", "Retrieves an AudioSource component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Values/Game Dev")]
    public class AudioSourceLookupValue : BaseComponentLookupValue<AudioSource, IAudioSourceValue>, IAudioSourceValue
    {
    }
}
