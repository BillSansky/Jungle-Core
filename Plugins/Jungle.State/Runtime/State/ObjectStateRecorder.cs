using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Static utility class for recording and restoring object states using custom SaveState classes.
    /// Fully generic and extensible - create custom state classes by inheriting from SaveState<T>.
    /// Only one state can be recorded per object at a time to prevent accidental overwrites.
    /// </summary>
    public static class ObjectStateRecorder
    {
        private static readonly Dictionary<int, ISaveState> RecordedStates = 
            new Dictionary<int, ISaveState>();

        #region Public API

        /// <summary>
        /// Records the current state of an object using the specified SaveState type.
        /// Only one state can be recorded per object. If a state already exists, this will fail with a warning.
        /// Use ClearState first if you need to replace an existing state.
        /// Usage: ObjectStateRecorder.Record<TransformPositionState>(myTransform);
        /// </summary>
        public static bool Record<TState>(UnityEngine.Object obj) where TState : ISaveState, new()
        {
            if (obj == null)
            {
                Debug.LogWarning($"Cannot record state: object is null.");
                return false;
            }

            int instanceId = obj.GetInstanceID();

            // Check if a state already exists
            if (RecordedStates.ContainsKey(instanceId))
            {
                Debug.LogWarning($"Cannot record state: object '{obj.name}' already has a recorded state of type {RecordedStates[instanceId].GetType().Name}. " +
                                $"Clear the existing state first with ClearState() before recording a new one.");
                return false;
            }

            TState state = new TState();
            if (!state.Capture(obj))
            {
                Debug.LogWarning($"Failed to capture state of type {typeof(TState).Name} for object '{obj.name}'.");
                return false;
            }

            RecordedStates[instanceId] = state;
            return true;
        }

      

        /// <summary>
        /// Records a new state, replacing any existing state for this object.
        /// Use this when you explicitly want to overwrite an existing state.
        /// </summary>
        public static bool RecordOrReplace<TState>(UnityEngine.Object obj) where TState : ISaveState, new()
        {
            if (obj == null)
            {
                Debug.LogWarning($"Cannot record state: object is null.");
                return false;
            }

            TState state = new TState();
            if (!state.Capture(obj))
            {
                Debug.LogWarning($"Failed to capture state of type {typeof(TState).Name} for object '{obj.name}'.");
                return false;
            }

            int instanceId = obj.GetInstanceID();
            RecordedStates[instanceId] = state;
            return true;
        }

        /// <summary>
        /// Restores the previously recorded state for the specified object.
        /// Usage: ObjectStateRecorder.Restore(myTransform);
        /// </summary>
        public static bool Restore(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                Debug.LogWarning($"Cannot restore state: object is null.");
                return false;
            }

            int instanceId = obj.GetInstanceID();

            if (!RecordedStates.ContainsKey(instanceId))
            {
                Debug.LogWarning($"No recorded state found for object '{obj.name}'.");
                return false;
            }

            return RecordedStates[instanceId].Restore(obj);
        }

        /// <summary>
        /// Restores the state only if it matches the specified type.
        /// Usage: ObjectStateRecorder.Restore<TransformPositionState>(myTransform);
        /// </summary>
        public static bool Restore<TState>(UnityEngine.Object obj) where TState : ISaveState
        {
            if (obj == null)
            {
                Debug.LogWarning($"Cannot restore state: object is null.");
                return false;
            }

            int instanceId = obj.GetInstanceID();

            if (!RecordedStates.ContainsKey(instanceId))
            {
                Debug.LogWarning($"No recorded state found for object '{obj.name}'.");
                return false;
            }

            ISaveState state = RecordedStates[instanceId];
            if (state.GetType() != typeof(TState))
            {
                Debug.LogWarning($"State type mismatch: expected {typeof(TState).Name} but found {state.GetType().Name} for object '{obj.name}'.");
                return false;
            }

            return state.Restore(obj);
        }

        /// <summary>
        /// Checks if any state has been recorded for the given object.
        /// </summary>
        public static bool HasState(UnityEngine.Object obj)
        {
            if (obj == null) return false;
            return RecordedStates.ContainsKey(obj.GetInstanceID());
        }

        /// <summary>
        /// Checks if a state of the specified type has been recorded for the given object.
        /// </summary>
        public static bool HasState<TState>(UnityEngine.Object obj) where TState : ISaveState
        {
            if (obj == null) return false;

            int instanceId = obj.GetInstanceID();
            return RecordedStates.ContainsKey(instanceId) && 
                   RecordedStates[instanceId].GetType() == typeof(TState);
        }

        /// <summary>
        /// Gets the type of the recorded state for the given object, or null if no state exists.
        /// </summary>
        public static Type GetStateType(UnityEngine.Object obj)
        {
            if (obj == null) return null;

            int instanceId = obj.GetInstanceID();
            return RecordedStates.ContainsKey(instanceId) ? RecordedStates[instanceId].GetType() : null;
        }

        /// <summary>
        /// Clears the recorded state for the given object.
        /// </summary>
        public static void ClearState(UnityEngine.Object obj)
        {
            if (obj == null) return;

            int instanceId = obj.GetInstanceID();
            RecordedStates.Remove(instanceId);
        }

        /// <summary>
        /// Clears all recorded states from all objects.
        /// </summary>
        public static void ClearAllStates()
        {
            RecordedStates.Clear();
        }

        #endregion
    }

    #region Save State System

    #endregion

    #region Built-in State Examples

    // ===== Transform States =====

    // ===== Rigidbody States =====

    // ===== Rigidbody2D States =====

    #endregion
}
