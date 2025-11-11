using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;
using UnityEngine.Playables;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Playable Director StartProcess Action",
        "Starts a PlayableDirector when the action begins and optionally stops it on stop.",
        "d_AnimationClip",
        "Actions/State")]
    /// <summary>
    /// Implements the playable director start action action.
    /// </summary>
    [Serializable]
    public class PlayableDirectorStartAction : IImmediateAction
    {
        [SerializeReference] private IGameObjectValue targetDirectorObject = new GameObjectValue();
        [SerializeField] private bool restartFromBeginning = true;


        private PlayableDirector cachedDirector;
        private bool hasExecuted;

        public void StartProcess(Action callback = null)
        {
            if (hasExecuted)
            {
                return;
            }

            var director = ResolveDirector();

            if (restartFromBeginning)
            {
                director.time = 0d;
            }

            director.Play();

            hasExecuted = true;
            callback?.Invoke();
        }

        public void Stop()
        {
            if (!hasExecuted)
            {
                return;
            }

            var director = cachedDirector ?? ResolveDirector();
            director.Stop();

            hasExecuted = false;
        }

        private PlayableDirector ResolveDirector()
        {
            Debug.Assert(targetDirectorObject != null, "PlayableDirector GameObject provider has not been assigned.");

            var gameObject = targetDirectorObject.V;
            Debug.Assert(gameObject != null, "The PlayableDirector GameObject provider returned a null instance.");

            cachedDirector = gameObject.GetComponent<PlayableDirector>();
            Debug.Assert(cachedDirector != null, $"PlayableDirector component was not found on '{gameObject.name}'.");

            return cachedDirector;
        }
    }
}
