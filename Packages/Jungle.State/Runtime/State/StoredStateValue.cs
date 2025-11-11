using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Utility;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns a recorded state of type <typeparamref name="TState"/> for a component.
    /// </summary>
    /// <typeparam name="TState">Type of the recorded state instance.</typeparam>
    public interface IStoredComponentStateValue<TState> : IValue<TState>
        where TState : class, ISaveState
    {
    }

    /// <summary>
    /// Retrieves a recorded state from the <see cref="ObjectStateRecorder"/> using a component reference.
    /// </summary>
    /// <typeparam name="TState">Type of the recorded state instance.</typeparam>
    [Serializable]
    [JungleClassInfo(
        "Stored Component State Value",
        "Returns the recorded state for a component captured by the ObjectStateRecorder.",
        null,
        "Values/Game Dev",
        true)]
    public class StoredComponentStateValue<TState> : IStoredComponentStateValue<TState>
        where TState : class, ISaveState
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IComponentValue))]
        private IComponentValue targetComponent = new ComponentLocalValue();

        /// <summary>
        /// Indicates whether multiple recorded states are available.
        /// </summary>
        public bool HasMultipleValues => targetComponent != null && targetComponent.HasMultipleValues;

        /// <summary>
        /// Retrieves all recorded states for the configured component providers.
        /// </summary>
        public IEnumerable<TState> Values
        {
            get
            {
                if (targetComponent == null)
                {
                    yield break;
                }

                foreach (var component in targetComponent.Values)
                {
                    if (component == null)
                    {
                        continue;
                    }

                    if (ObjectStateRecorder.TryGetState(component, out TState state))
                    {
                        yield return state;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the recorded state associated with the first component produced by the provider.
        /// </summary>
        public TState Value()
        {
            if (targetComponent == null)
            {
                return null;
            }

            Component component = targetComponent.Value();
            if (component == null)
            {
                return null;
            }

            return ObjectStateRecorder.TryGetState(component, out TState state) ? state : null;
        }
    }
}
