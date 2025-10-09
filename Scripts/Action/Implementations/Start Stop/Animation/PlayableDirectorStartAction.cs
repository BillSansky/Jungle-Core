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
    [Serializable]
    public class PlayableDirectorStartAction : BeginCompleteAction
    {
        [SerializeReference] private IGameObjectValue targetDirectorObject = new GameObjectValue();
        [SerializeField] private bool restartFromBeginning = true;
        [SerializeField] private bool stopOnActionStop = true;

        private PlayableDirector cachedDirector;

        public override bool IsTimed => false;
        public override float Duration => 0f;

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
            var director = ResolveDirector();

            if (restartFromBeginning)
            {
                director.time = 0d;
            }

            director.Play();
        }

        protected override void OnStop()
        {
            if (!stopOnActionStop)
            {
                return;
            }

            var director = cachedDirector ?? ResolveDirector();
            director.Stop();
        }

        private PlayableDirector ResolveDirector()
        {
            if (targetDirectorObject == null)
            {
                throw new InvalidOperationException("PlayableDirector GameObject provider has not been assigned.");
            }

            var gameObject = targetDirectorObject.V;
            if (gameObject == null)
            {
                throw new InvalidOperationException("The PlayableDirector GameObject provider returned a null instance.");
            }

            cachedDirector = gameObject.GetComponent<PlayableDirector>();
            if (cachedDirector == null)
            {
                throw new InvalidOperationException($"PlayableDirector component was not found on '{gameObject.name}'.");
            }

            return cachedDirector;
        }
    }
}
