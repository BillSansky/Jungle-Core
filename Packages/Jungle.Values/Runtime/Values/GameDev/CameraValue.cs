using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to a Camera reference.
    /// </summary>
    public interface ICameraValue : IValue<Camera>
    {
    }
    public interface ISettableCameraValue : ICameraValue, IValueSableValue<Camera>
    {
    }

    /// <summary>
    /// Stores a camera component directly on the owner.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera Value", "Stores a camera component directly on the owner.", null, "Game Dev", true)]
    public class CameraValue : LocalValue<Camera>, ISettableCameraValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
    }

    [Serializable]
    public class MainCamera : ICameraValue
    {
        private Camera cachedCamera;
        
        public Camera Value()
        {
            if(!cachedCamera)
                cachedCamera=Camera.main;
            return Camera.main;
        }

        public bool HasMultipleValues => false;
    }
    
    /// <summary>
    /// Returns a camera component from a component field, property, or method.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera Member Value", "Returns a camera component from a component field, property, or method.", null, "Game Dev")]
    public class CameraClassMembersValue : ClassMembersValue<Camera>, ICameraValue
    {
    }
}
