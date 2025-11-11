using System;

namespace Jungle.Events
{
    /// <summary>
    /// Represents a schedulable callback that can notify subscribers once its condition is met.
    /// Implementations can wait for timers, input signals, ScriptableObject events and more.
    /// Additional variations such as animation events, physics triggers, timeline markers or network messages can be
    /// implemented by following the same pattern.
    /// </summary>
    public interface ICallback
    {
        /// <summary>
        /// Registers a callback action that will be invoked when the callback completes.
        /// </summary>
        /// <param name="callbackAction">Callback action to register.</param>
        void Attach(Action callbackAction);

        /// <summary>
        /// Removes a callback action previously added via <see cref="Attach"/>.
        /// </summary>
        /// <param name="callbackAction">Callback action to remove.</param>
        void Detach(Action callbackAction);
        
    }
}
