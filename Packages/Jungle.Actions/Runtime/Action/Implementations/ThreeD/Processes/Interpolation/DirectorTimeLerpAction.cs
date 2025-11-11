using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;
using UnityEngine.Playables;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Director Time Lerp Process",
        "Lerps a PlayableDirector's time from start to end using interpolation curves and loop strategies.",
        "d_AnimationClip",
        "Actions/Process")]
    /// <summary>
    /// Implements the director time lerp action action.
    /// </summary>
    [Serializable]
    public class DirectorTimeLerpAction : LerpProcessAction<double>
    {
        [SerializeReference] private IGameObjectValue targetDirectorObject = new GameObjectValue();

        protected override double GetStartValue()
        {
            return 0d;
        }

        protected override double GetEndValue()
        {
            var director = ResolveDirector();
            return director.duration;
        }

        protected override double LerpValue(double start, double end, float t)
        {
            return start + (end - start) * t;
        }

        protected override void ApplyValue(double value)
        {
            var director = ResolveDirector();
            director.time = value;
        }

        protected override void OnBeforeStart()
        {
            var director = ResolveDirector();
            director.time = 0d;
            director.Play();
        }

        protected override void OnInterrupted()
        {
            var director = ResolveDirector();
            director.Stop();
        }

        private PlayableDirector ResolveDirector()
        {
            Debug.Assert(targetDirectorObject != null, "PlayableDirector GameObject provider has not been assigned.");

            var gameObject = targetDirectorObject.V;
            Debug.Assert(gameObject != null, "The PlayableDirector GameObject provider returned a null instance.");

            var director = gameObject.GetComponent<PlayableDirector>();
            Debug.Assert(director != null, $"PlayableDirector component was not found on '{gameObject.name}'.");

            return director;
        }
    }
}
