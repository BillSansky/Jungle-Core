using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Computes a normalized direction vector from a camera to a world position.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Camera Ray Direction To Position", "Computes a normalized direction vector from a camera to a world position.", null, "Unity Types")]
    public class CameraRayDirectionToPositionValue : IVector3Value
    {
        /// <summary>
        /// References the camera used as the ray origin.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection]
        public ICameraValue camera = new CameraValue();

        /// <summary>
        /// References the world position used as the ray target.
        /// </summary>
        [SerializeReference]
        [JungleClassSelection]
        public IVector3Value targetPosition = new Vector3Value();

        /// <inheritdoc />
        public Vector3 Value()
        {
            Camera cameraValue = camera.Value();
            Vector3 position = targetPosition.Value();
            return CalculateDirection(cameraValue, position);
        }

        /// <inheritdoc />
        public bool HasMultipleValues => camera.HasMultipleValues || targetPosition.HasMultipleValues;

        /// <inheritdoc />
        public IEnumerable<Vector3> Values
        {
            get
            {
                foreach (Camera cameraValue in camera.Values)
                {
                    foreach (Vector3 position in targetPosition.Values)
                    {
                        yield return CalculateDirection(cameraValue, position);
                    }
                }
            }
        }

        private static Vector3 CalculateDirection(Camera cameraValue, Vector3 position)
        {
            Vector3 origin = cameraValue.transform.position;
            return (position - origin).normalized;
        }
    }
}
