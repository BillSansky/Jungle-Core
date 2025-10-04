using Jungle.Actions;

using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class AudioAction : StartStopAction
    {
        [SerializeReference] private FlexibleAudioSourceValue audioSource;
        [SerializeField] private AudioClip dragStartSound;
        [SerializeField] private AudioClip dragEndSound;
        [SerializeField] private AudioClip targetReachedSound;

       

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
            if (!dragStartSound) return;

            foreach (var source in audioSource.Value)
            {
                if (source != null)
                    source.PlayOneShot(dragStartSound);
            }
        }

        protected override void OnStop()
        {
            if (!dragEndSound) return;

            foreach (var source in audioSource.Value)
            {
                if (source != null)
                    source.PlayOneShot(dragEndSound);
            }
        }

        // Additional method for target reached functionality
        public override void OneShot(DraggableObject draggableInContext, DragZone dragZoneInContext)
        {
            UpdateContext(dragZoneInContext, draggableInContext);
            if (!targetReachedSound) return;

            foreach (var source in audioSource.Value)
            {
                if (source != null)
                    source.PlayOneShot(targetReachedSound);
            }
        }

        public override void UpdateContext(DragZone dragZone, DraggableObject draggable)
        {
            audioSource.UpdateContext(dragZone, draggable);
        }
    }
}
