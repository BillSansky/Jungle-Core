using System;
using Jungle.Attributes;
using Jungle.Values;
using Jungle.Values.GameDev;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Casts a physics ray and stores the resulting hit information.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Raycast Hit Capture",
        "Casts a physics ray and stores the resulting hit information in collision hit and collider values.",
        null,
        "Actions/State")]
    public class RaycastHitCaptureAction : IImmediateAction
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IVector3Value))]
        private IVector3Value rayOrigin = new Vector3Value();

        [SerializeReference]
        [JungleClassSelection(typeof(IVector3Value))]
        private IVector3Value rayDirection = new Vector3Value();

        [SerializeReference]
        [JungleClassSelection(typeof(ILayerMaskValue))]
        private ILayerMaskValue rayLayerMask = new LayerMaskValue();

        [SerializeReference]
        [JungleClassSelection(typeof(ICollisionHitValue))]
        private ICollisionHitValue hitStorage = new CollisionHitLocalValue();

        [SerializeReference]
        [JungleClassSelection(typeof(IColliderValue))]
        private IColliderValue colliderStorage = new ColliderLocalValue();

        [SerializeField]
        private float maxDistance = 100f;

        [SerializeField]
        private bool normalizeDirection = true;

        [SerializeField]
        private bool clearValuesWhenNoHit = true;

        /// <inheritdoc />
        public void StartProcess(Action callback = null)
        {
            CaptureHit();
            callback?.Invoke();
        }

        /// <inheritdoc />
        public void Stop()
        {
        }

        private void CaptureHit()
        {
            Vector3 origin = rayOrigin.Value();
            Vector3 direction = rayDirection.Value();

            if (direction.sqrMagnitude == 0f)
            {
                Debug.LogWarning($"{nameof(RaycastHitCaptureAction)}: Ray direction has zero magnitude.");
                return;
            }

            if (normalizeDirection)
            {
                direction.Normalize();
            }

            float distance = Mathf.Max(0f, maxDistance);
            LayerMask layerMask = rayLayerMask.Value();

            bool hasHit = UnityEngine.Physics.Raycast(origin, direction, out RaycastHit hit, distance, layerMask);

            AssignHitResults(hasHit, hit);
        }

        private void AssignHitResults(bool hasHit, RaycastHit hit)
        {
            if (hitStorage is ISettableValue<RaycastHit> settableHit)
            {
                if (hasHit)
                {
                    settableHit.SetValue(hit);
                }
                else if (clearValuesWhenNoHit)
                {
                    settableHit.SetValue(default);
                }
            }
            else
            {
                Debug.LogWarning($"{nameof(RaycastHitCaptureAction)}: Configured collision hit value does not allow assignments.");
            }

            if (colliderStorage is ISettableValue<Collider> settableCollider)
            {
                if (hasHit)
                {
                    settableCollider.SetValue(hit.collider);
                }
                else if (clearValuesWhenNoHit)
                {
                    settableCollider.SetValue(null);
                }
            }
            else
            {
                Debug.LogWarning($"{nameof(RaycastHitCaptureAction)}: Configured collider value does not allow assignments.");
            }
        }
    }
}
