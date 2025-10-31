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
        [SerializeReference] private IAudioSourceValue audioSource;
        [SerializeField] private AudioClip dragStartSound;
        [SerializeField] private AudioClip dragEndSound;
        [SerializeField] private AudioClip targetReachedSound;

        /// <summary>
        /// Plays the configured start clip on every resolved AudioSource when the state begins.
        /// </summary>
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
        /// Plays the configured end clip when the state stops.
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
