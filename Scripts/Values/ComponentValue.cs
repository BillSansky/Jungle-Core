using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="Component"/> that implements <see cref="IValue{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    [Serializable]
    public class ComponentValue<T> : ValueSource<T>
    {
        [SerializeField]
        private Component component;

        /// <inheritdoc />
        public override T GetValue()
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
