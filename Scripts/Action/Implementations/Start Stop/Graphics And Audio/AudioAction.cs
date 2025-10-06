using Jungle.Actions;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class AudioAction : ProcessAction
    {
        [SerializeReference] private IAudioSourceValue audioSource;
        [SerializeField] private AudioClip dragStartSound;
        [SerializeField] private AudioClip dragEndSound;
        [SerializeField] private AudioClip targetReachedSound;

        public override bool IsTimed => false;
        public override float Duration => 0f;
        
        protected override void BeginImpl()
        {
            if (!dragStartSound) return;

            foreach (var source in audioSource.Values)
            {
                if (source != null)
                    source.PlayOneShot(dragStartSound);
            }
        }

        protected override void CompleteImpl()
        {
            if (!dragEndSound) return;

            foreach (var source in audioSource.Values)
            {
                if (source != null)
                    source.PlayOneShot(dragEndSound);
            }
        }

    }
}
