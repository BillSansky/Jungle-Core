using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves a Camera component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera Lookup Value", "Retrieves a Camera component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Values/Game Dev")]
    public class CameraLookupValue : BaseComponentLookupValue<Camera, ICameraValue>, ICameraValue
    {
    }
}
