using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Switches Rigidbody components between kinematic and simulated modes for the state.
    /// </summary>
    [System.Serializable]
    public class RigidbodyKinematicStateAction : IStateAction
    {
        /// <summary>
        /// Defines how the kinematic flag should change during the state lifecycle.
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
        [SerializeField] private KinematicOption endAction = KinematicOption.None;
        private List<bool> wasKinematic = new();
        /// <summary>
        /// Caches the current kinematic state and applies the entry rule to each body.
        /// </summary>
        public void OnStateEnter()
        {
            wasKinematic.Clear();


            foreach (var rb in targetRigidbodies.Values)
            {
                wasKinematic.Add(rb.isKinematic);
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
                    case KinematicOption.Toggle:
                        rb.isKinematic = !rb.isKinematic;
                        break;
                }
            }
        }
        /// <summary>
        /// Applies the configured exit rule so rigidbodies leave in the desired mode.
        /// </summary>
        public void OnStateExit()
        {
            int i = 0;
            foreach (var rb in targetRigidbodies.Values)
            {
                switch (endAction)
                {
                    case KinematicOption.None:
                        rb.isKinematic = wasKinematic[i];
                        break;
                    case KinematicOption.Kinematic:
                        rb.isKinematic = true;
                        break;
                    case KinematicOption.NonKinematic:
                        rb.isKinematic = false;
                        break;
                    case KinematicOption.Original:
                        rb.isKinematic = wasKinematic[i];
                        break;
                    case KinematicOption.Toggle:
                        rb.isKinematic = !rb.isKinematic;
                        break;
                }

                i++;
            }
        }
    }
}