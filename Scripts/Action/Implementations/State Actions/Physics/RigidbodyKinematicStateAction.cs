using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    [JungleClassInfo("Rigidbody Kinematic Action", "Adjusts the kinematic state of rigidbodies at state start and stop.", null, "Actions/State")]
    public class RigidbodyKinematicStateAction : IStateAction
    {
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