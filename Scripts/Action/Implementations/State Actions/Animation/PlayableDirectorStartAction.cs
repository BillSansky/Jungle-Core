using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;
using UnityEngine.Playables;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Starts a PlayableDirector when the action begins and optionally stops it on stop.",
        "d_AnimationClip")]
    /// <summary>
    /// Starts a Timeline PlayableDirector and optionally rewinds it when the state ends.
    /// </summary>
    [Serializable]
    public class PlayableDirectorStartAction : IStateAction
    {
        [SerializeReference] private IGameObjectValue targetDirectorObject = new GameObjectValue();
        [SerializeField] private bool restartFromBeginning = true;


        private PlayableDirector cachedDirector;
        /// <summary>
        /// Starts the timeline playback when the state begins, optionally rewinding first.
        /// </summary>
        public void OnStateEnter()
        {
            var director = ResolveDirector();

            if (restartFromBeginning)
            {
                director.time = 0d;
            }

            director.Play();
        }
        /// <summary>
        /// Stops the director when the state ends so it does not keep running in the background.
        /// </summary>
        public void OnStateExit()
        {

            var director = cachedDirector ?? ResolveDirector();
            director.Stop();
        }
        /// <summary>
        /// Locates and caches the PlayableDirector component to control.
        /// </summary>
        private PlayableDirector ResolveDirector()
        {
            Debug.Assert(targetDirectorObject != null, "PlayableDirector GameObject provider has not been assigned.");

            var gameObject = targetDirectorObject.G;
            Debug.Assert(gameObject != null, "The PlayableDirector GameObject provider returned a null instance.");

            cachedDirector = gameObject.GetComponent<PlayableDirector>();
            Debug.Assert(cachedDirector != null, $"PlayableDirector component was not found on '{gameObject.name}'.");

            return cachedDirector;
        }
    }
}
