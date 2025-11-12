using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a <see cref="Ray"/>.
    /// </summary>
    public interface IRayValue : IValue<Ray>
    {
    }

    /// <summary>
    /// Represents a value provider that returns a <see cref="Ray"/> and supports setting it.
    /// </summary>
    public interface ISettableRayValue : IRayValue, IValueSableValue<Ray>
    {
    }

    /// <summary>
    /// Stores a <see cref="Ray"/> locally on the owner.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Ray Value", "Stores a ray locally on the owner.", null, "Unity Types", true)]
    public class RayValue : LocalValue<Ray>, ISettableRayValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RayValue"/> class.
        /// </summary>
        public RayValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RayValue"/> class.
        /// </summary>
        /// <param name="value">Initial ray value.</param>
        public RayValue(Ray value)
            : base(value)
        {
        }

        /// <inheritdoc />
        public override bool HasMultipleValues => false;
    }

    /// <summary>
    /// Creates a <see cref="Ray"/> from a camera using a screen position.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Ray From Camera", "Creates a ray from a camera using a screen position.", null, "Unity Types")]
    public class RayFromCameraValue : IRayValue
    {
        /// <summary>
        /// References the camera that acts as the ray origin.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection(typeof(ICameraValue))]
        public ICameraValue camera = new CameraValue();

        /// <summary>
        /// References the screen position used to build the ray.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection(typeof(IVector3Value))]
        public IVector3Value screenPosition = new Vector3Value();

        /// <inheritdoc />
        public Ray Value()
        {
            Camera cameraValue = camera.Value();
            Vector3 screenPoint = screenPosition.Value();
            return cameraValue.ScreenPointToRay(screenPoint);
        }

        /// <inheritdoc />
        public bool HasMultipleValues => camera.HasMultipleValues || screenPosition.HasMultipleValues;

        /// <inheritdoc />
        public IEnumerable<Ray> Values
        {
            get
            {
                foreach (Camera cameraValue in camera.Values)
                {
                    foreach (Vector3 screenPoint in screenPosition.Values)
                    {
                        yield return cameraValue.ScreenPointToRay(screenPoint);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a <see cref="Ray"/> from an origin and direction.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Ray From Origin And Direction", "Creates a ray using an origin and direction.", null, "Unity Types")]
    public class RayFromOriginDirectionValue : IRayValue
    {
        /// <summary>
        /// References the origin of the ray.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection(typeof(IVector3Value))]
        public IVector3Value origin = new Vector3Value();

        /// <summary>
        /// References the direction of the ray.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection(typeof(IVector3Value))]
        public IVector3Value direction = new Vector3Value();

        /// <inheritdoc />
        public Ray Value()
        {
            Vector3 originValue = origin.Value();
            Vector3 directionValue = direction.Value();
            return new Ray(originValue, directionValue);
        }

        /// <inheritdoc />
        public bool HasMultipleValues => origin.HasMultipleValues || direction.HasMultipleValues;

        /// <inheritdoc />
        public IEnumerable<Ray> Values
        {
            get
            {
                foreach (Vector3 originValue in origin.Values)
                {
                    foreach (Vector3 directionValue in direction.Values)
                    {
                        yield return new Ray(originValue, directionValue);
                    }
                }
            }
        }
    }
}
