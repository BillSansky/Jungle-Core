using Jungle.Actions;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Plays or stops an AudioSource when the state transitions.
    /// </summary>
    [System.Serializable]
    public class AudioAction : IStateAction
    {
        /// <summary>
        /// Handles the OnStateEnter event.
        /// </summary>
        [SerializeReference] private IAudioSourceValue audioSource;
        [SerializeField] private AudioClip dragStartSound;
        [SerializeField] private AudioClip dragEndSound;
        [SerializeField] private AudioClip targetReachedSound;

        public void OnStateEnter()
        {
            if (!dragStartSound) return;

            foreach (var source in audioSource.Values)
            {
                if (source != null)
                    source.PlayOneShot(dragStartSound);
            }
        }
        /// <summary>
        /// Handles the OnStateExit event.
        /// </summary>
        public void OnStateExit()
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
