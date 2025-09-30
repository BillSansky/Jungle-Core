using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="Component"/> that implements <see cref="IValue{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    [Serializable]
    public class ValueFromComponent<T,T1> : IValue<T> where T1: ValueComponent<T>
    {
        [SerializeField]
        private T1 component;

        /// <inheritdoc />
        public T GetValue()
        {
            if (component == null)
            {
                Debug.LogError(
                    $"Component reference is missing for {GetType().Name}. Assign a component that implements {typeof(IValue<T>).Name}.");
                return default;
            }

            if (component is IValue<T> valueProvider)
            {
                return valueProvider.GetValue();
            }

            Debug.LogError(
                $"Component {component.GetType().Name} does not implement {typeof(IValue<T>).Name} and cannot provide the expected value.");
            return default;
        }
    }
}
