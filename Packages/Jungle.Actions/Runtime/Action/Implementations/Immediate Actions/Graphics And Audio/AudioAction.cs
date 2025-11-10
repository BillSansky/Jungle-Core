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

        private bool hasExecuted;

        public void Start(Action callback = null)
        {
            if (hasExecuted)
            {
                return;
            }

            PlayClip(dragStartSound);

            hasExecuted = true;
            callback?.Invoke();
        }

        public void Stop()
        {
            if (!hasExecuted)
            {
                return;
            }

            PlayClip(dragEndSound);

            hasExecuted = false;
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
