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
    public class PlayableDirectorStartAction : IBeginEndAction
    {
        [SerializeReference] private IGameObjectValue targetDirectorObject = new GameObjectValue();
        [SerializeField] private bool restartFromBeginning = true;
       

        private PlayableDirector cachedDirector;

        public void Begin()
        {
            var director = ResolveDirector();

            if (restartFromBeginning)
            {
                director.time = 0d;
            }

            director.Play();
        }

        public void End()
        {
           
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
