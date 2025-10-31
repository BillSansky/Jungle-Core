using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;
using UnityEngine.Playables;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Lerps a PlayableDirector's time from start to end using interpolation curves and loop strategies.",
        "d_AnimationClip")]
    /// <summary>
    /// Animates a PlayableDirector time value to smoothly scrub a timeline.
    /// </summary>
    [Serializable]
    public class DirectorTimeLerpAction : LerpProcessAction<double>
    {
        /// <summary>
        /// Always begins playback from the director's start time.
        /// </summary>
        [SerializeReference] private IGameObjectValue targetDirectorObject = new GameObjectValue();

        protected override double GetStartValue()
        {
            return 0d;
        }
        /// <summary>
        /// Uses the director's configured duration as the interpolation destination.
        /// </summary>
        protected override double GetEndValue()
        {
            var director = ResolveDirector();
            return director.duration;
        }
        /// <summary>
        /// Linearly interpolates between the current and final director times.
        /// </summary>
        protected override double LerpValue(double start, double end, float t)
        {
            return start + (end - start) * t;
        }
        /// <summary>
        /// Sets the director's playback time to the interpolated value.
        /// </summary>
        protected override void ApplyValue(double value)
        {
            var director = ResolveDirector();
            director.time = value;
        }
        /// <summary>
        /// Rewinds and starts the director so the timeline scrubs from the beginning.
        /// </summary>
        protected override void OnBeforeStart()
        {
            var director = ResolveDirector();
            director.time = 0d;
            director.Play();
        }
        /// <summary>
        /// Stops the director if the tween is cancelled before completion.
        /// </summary>
        protected override void OnInterrupted()
        {
            var director = ResolveDirector();
            director.Stop();
        }
        /// <summary>
        /// Retrieves the PlayableDirector component that should be driven by the action.
        /// </summary>
        private PlayableDirector ResolveDirector()
        {
            Debug.Assert(targetDirectorObject != null, "PlayableDirector GameObject provider has not been assigned.");

            var gameObject = targetDirectorObject.G;
            Debug.Assert(gameObject != null, "The PlayableDirector GameObject provider returned a null instance.");

            var director = gameObject.GetComponent<PlayableDirector>();
            Debug.Assert(director != null, $"PlayableDirector component was not found on '{gameObject.name}'.");

            return director;
        }
    }
}
