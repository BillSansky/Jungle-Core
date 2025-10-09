using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class RigidbodyKinematikSetterAction : BeginCompleteAction
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
        private IRigidbodyValue targetRigidbody = new RigidbodyValue();

        [SerializeField] private KinematicOption ActionStart = KinematicOption.Kinematic;
        [SerializeField] private KinematicOption ActionEnd = KinematicOption.None;
        private bool wasKinematic;
        private bool skipStop;

        public override bool IsTimed => false;
        public override float Duration => 0f;

        public void StartAction()
        {
            Start();
        }

        public void StopAction()
        {
            Stop();
        }

        protected override void OnStart()
        {
            var rb = (Rigidbody)targetRigidbody;
            if (!rb) return;

            wasKinematic = rb.isKinematic;
            switch (ActionStart)
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
                    rb.isKinematic = !wasKinematic;
                    break;
            }
        }

        protected override void OnStop()
        {
            if (skipStop)
            {
                return;
            }

            var rb = (Rigidbody)targetRigidbody;
            if (!rb) return;

            switch (ActionEnd)
            {
                case KinematicOption.None:
                    rb.isKinematic = wasKinematic;
                    break;
                case KinematicOption.Kinematic:
                    rb.isKinematic = true;
                    break;
                case KinematicOption.NonKinematic:
                    rb.isKinematic = false;
                    break;
                case KinematicOption.Original:
                    rb.isKinematic = wasKinematic;
                    break;
                case KinematicOption.Toggle:
                    rb.isKinematic = !rb.isKinematic;
                    break;
            }
        }
    }
}