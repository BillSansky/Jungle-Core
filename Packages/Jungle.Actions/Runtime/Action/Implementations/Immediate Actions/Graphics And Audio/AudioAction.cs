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
    public class AudioAction : IStateAction
    {
        /// <summary>
        /// Invoked when the state becomes active.
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
        /// Invoked when the state finishes running.
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
