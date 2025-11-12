using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a camera component.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Camera value", fileName = "CameraValue")]
    [JungleClassInfo("Camera Value Asset", "ScriptableObject storing a camera component.", null, "Game Dev")]
    public class CameraValueAsset : ValueAsset<Camera>
    {
        [SerializeField]
        private Camera value;

        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>
        public override Camera Value()
        {
            return value;
        }

        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>
        public override void SetValue(Camera value)
        {
            this.value = value;
        }
    }

    /// <summary>
    /// ScriptableObject storing a list of camera components.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Camera list value", fileName = "CameraListValue")]
    [JungleClassInfo("Camera List Asset", "ScriptableObject storing a list of camera components.", null, "Game Dev")]
    public class CameraListValueAsset : SerializedValueListAsset<Camera>
    {
    }

    /// <summary>
    /// Reads a camera component from a CameraValueAsset.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera Value From Asset", "Reads a camera component from a CameraValueAsset.", null, "Game Dev")]
    public class CameraValueFromAsset : ValueFromAsset<Camera, CameraValueAsset>, ISettableCameraValue
    {
    }

    /// <summary>
    /// Reads camera components from a CameraListValueAsset.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera List From Asset", "Reads camera components from a CameraListValueAsset.", null, "Game Dev")]
    public class CameraListValueFromAsset : ValueFromAsset<IReadOnlyList<Camera>, CameraListValueAsset>
    {
    }
}
