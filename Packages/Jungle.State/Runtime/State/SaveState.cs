using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Abstract base class for creating save states.
    /// Inherit from this class to create custom state types for any component.
    /// Override OnCheckConflict to define what properties this state affects.
    /// </summary>
    public abstract class SaveState<T> : ISaveState where T : UnityEngine.Object
    {
        public bool Capture(UnityEngine.Object obj)
        {
            if (obj is T target)
            {
                OnCapture(target);
                return true;
            }

            Debug.LogWarning($"Cannot capture: object is not of type {typeof(T).Name}");
            return false;
        }

        public bool Restore(UnityEngine.Object obj)
        {
            if (obj is T target)
            {
                OnRestore(target);
                return true;
            }

            Debug.LogWarning($"Cannot restore: object is not of type {typeof(T).Name}");
            return false;
        }

        public bool IsConflictingWith(ISaveState otherState)
        {
            // States of the same exact type always conflict
            if (otherState.GetType() == this.GetType())
            {
                return true;
            }

            // Delegate to the custom conflict check
            return OnCheckConflict(otherState);
        }

        protected abstract void OnCapture(T target);
        protected abstract void OnRestore(T target);

        /// <summary>
        /// Override this to define conflict detection logic.
        /// Return true if this state modifies the same properties as the other state.
        /// By default, states only conflict if they're the exact same type.
        /// </summary>
        protected virtual bool OnCheckConflict(ISaveState otherState)
        {
            return false;
        }
    }
}