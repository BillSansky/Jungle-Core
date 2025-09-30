using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueComponent{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    /// <typeparam name="TComponent">Concrete component type used to fetch the value.</typeparam>
    [Serializable]
    public class ValueFromComponent<T, TComponent> : IValue<T>
        where TComponent : ValueComponent<T>
    {
        [SerializeField]
        private TComponent component;

        /// <inheritdoc />
        public T GetValue()
        {
            if (component == null)
            {
                Debug.LogError(
                    $"Component reference is missing for {GetType().Name}. Assign a {typeof(TComponent).Name} instance.");
                return default;
            }

            return component.GetValue();
        }
    }
}
