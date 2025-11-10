using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;
using UnityEngine.Playables;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Playable Director Start Action",
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

            var director = ResolveDirector();

            if (restartFromBeginning)
            {
                director.time = 0d;
            }

            director.Play();

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

            var director = cachedDirector ?? ResolveDirector();
            director.Stop();

            isInProgress = false;
            hasCompleted = false;
        }

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
