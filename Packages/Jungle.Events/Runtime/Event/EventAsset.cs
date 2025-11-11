using System;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// ScriptableObject-based event asset that allows loosely-coupled systems to communicate
    /// through explicit registration of listeners.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Events/Event Asset", fileName = "EventAsset")]
    public class EventAsset : ScriptableObject
    {
        private event Action listeners = delegate { };

        /// <summary>
        /// Registers a <paramref name="listener"/> that will be invoked whenever the event is raised.
        /// </summary>
        /// <param name="listener">Callback to register.</param>
        public void Register(Action listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            listeners += listener;
        }

        /// <summary>
        /// Removes a previously registered <paramref name="listener"/>.
        /// </summary>
        /// <param name="listener">Callback to unregister.</param>
        public void Unregister(Action listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            listeners -= listener;
        }

        /// <summary>
        /// Raises the event, invoking all registered listeners.
        /// </summary>
        public void Raise()
        {
            listeners.Invoke();
        }

        /// <summary>
        /// Creates an <see cref="ICallback"/> that completes when this event asset is raised.
        /// </summary>
        /// <returns>A callback instance that can wait for the event.</returns>
        public ICallback CreateCallback()
        {
            return new EventAssetCallback(this);
        }
    }
}
