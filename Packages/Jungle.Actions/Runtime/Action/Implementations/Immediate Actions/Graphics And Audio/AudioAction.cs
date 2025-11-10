using System;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Plays configured audio clips when entering and exiting the state.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Audio State Action", "Plays configured audio clips when entering and exiting the state.", null, "Actions/State")]
    public class AudioAction : IImmediateAction
    {
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>
        [SerializeReference] private IAudioSourceValue audioSource;
        [SerializeField] private AudioClip dragStartSound;
        [SerializeField] private AudioClip dragEndSound;
        [SerializeField] private AudioClip targetReachedSound;

        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration => 0f;

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Start(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            PlayClip(dragStartSound);

            isInProgress = true;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            PlayClip(dragEndSound);

            isInProgress = false;
            hasCompleted = false;
        }

        private void PlayClip(AudioClip clip)
        {
            if (!clip)
            {
                return;
            }

            foreach (var source in audioSource.Values)
            {
                if (source != null)
                {
                    source.PlayOneShot(clip);
                }
            }
        }

    }
}
