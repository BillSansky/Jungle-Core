using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.UnityTypes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Jungle.Actions
{
    [Serializable]
    [JungleClassInfo(
        "Move Transform To Raycast",
        "Casts a ray from a camera and moves the target transform to the hit position or a fallback distance.",
        "d_SceneViewOrtho",
        "Actions/Physics")]
    public class TransformRaycastMoveAction : IImmediateAction
    {
        [SerializeReference]
        [JungleClassSelection]
        private ITransformValue targetTransform = new TransformLocalValue();

        [SerializeReference]
        [JungleClassSelection(typeof(IVector3Value))]
        private IVector3Value rayOrigin = new Vector3Value();

        [SerializeReference]
        [JungleClassSelection(typeof(ICameraValue))]
        private ICameraValue cameraProvider = new CameraValue();

        [SerializeReference]
        [JungleClassSelection(typeof(ILayerMaskValue))]
        private ILayerMaskValue layerMaskProvider = new LayerMaskValue();

        [SerializeField]
        private float maxDistance = 100f;

        [SerializeField]
        private float distanceWhenNoHit = 5f;

        public void StartProcess(Action callback = null)
        {
            MoveTargetsToRaycastPosition();
            callback?.Invoke();
        }

        public void Stop()
        {
        }

        private void MoveTargetsToRaycastPosition()
        {
            var camera = cameraProvider.Value();
            Assert.IsNotNull(camera, $"{nameof(TransformRaycastMoveAction)} requires a valid camera instance.");

            Vector3 direction = camera.transform.forward;
            if (direction.sqrMagnitude == 0f)
            {
                Debug.LogWarning($"{nameof(TransformRaycastMoveAction)}: Camera forward vector has zero magnitude.");
                return;
            }

            Vector3 origin = rayOrigin.Value();
            direction.Normalize();
            float rayDistance = Mathf.Max(0f, maxDistance);
            LayerMask layerMask = layerMaskProvider.Value();

            bool hasHit = UnityEngine.Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance, layerMask);
            Vector3 targetPosition = hasHit ? hit.point : origin + direction * Mathf.Max(0f, distanceWhenNoHit);

            foreach (Transform transform in targetTransform.Values)
            {
                Assert.IsNotNull(transform, $"{nameof(TransformRaycastMoveAction)} requires non-null transform targets.");
                transform.position = targetPosition;
            }
        }
    }
}
