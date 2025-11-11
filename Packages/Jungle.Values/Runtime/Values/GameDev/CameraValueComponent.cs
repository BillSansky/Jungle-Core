using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a camera component.
    /// </summary>
    [JungleClassInfo("Camera Value Component", "Component exposing a camera component.", null, "Values/Game Dev")]
    public class CameraValueComponent : ValueComponent<Camera>
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
    /// Component exposing a list of camera components.
    /// </summary>
    [JungleClassInfo("Camera List Component", "Component exposing a list of camera components.", null, "Values/Game Dev")]
    public class CameraListValueComponent : SerializedValueListComponent<Camera>
    {
    }

    /// <summary>
    /// Reads a camera component from a CameraValueComponent.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera Value From Component", "Reads a camera component from a CameraValueComponent.", null, "Values/Game Dev")]
    public class CameraValueFromComponent : ValueFromComponent<Camera, CameraValueComponent>, ICameraValue
    {
    }

    /// <summary>
    /// Reads camera components from a CameraListValueComponent.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera List From Component", "Reads camera components from a CameraListValueComponent.", null, "Values/Game Dev")]
    public class CameraListValueFromComponent : ValueFromComponent<IReadOnlyList<Camera>, CameraListValueComponent>
    {
    }
}
