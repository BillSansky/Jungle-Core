using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Adjusts the kinematic state of rigidbodies when the action executes.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Rigidbody Kinematic Action", "Adjusts the kinematic state of rigidbodies when the action executes.", null, "Actions/State")]
    public class RigidbodyKinematicStateAction : IImmediateAction
    {
        /// <summary>
        /// Implements the kinematic option action.
        /// </summary>
        public enum KinematicOption
        {
            None,
            Kinematic,
            NonKinematic,
            Original,
            Toggle
        }

        [SerializeReference] [JungleClassSelection]
        private IRigidbodyValue targetRigidbodies = new RigidbodyValue();

        [SerializeField] private KinematicOption beginAction = KinematicOption.Kinematic;

        public void StartProcess(Action callback = null)
        {
            foreach (var rb in targetRigidbodies.Values)
            {
                switch (beginAction)
                {
                    case KinematicOption.None:
                        break;
                    case KinematicOption.Kinematic:
                        rb.isKinematic = true;
                        break;
                    case KinematicOption.NonKinematic:
                        rb.isKinematic = false;
                        break;
                    case KinematicOption.Original:
                        break;
                    case KinematicOption.Toggle:
                        rb.isKinematic = !rb.isKinematic;
                        break;
                }
            }

            callback?.Invoke();
        }
    }
}
