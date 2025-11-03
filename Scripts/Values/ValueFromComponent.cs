using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueComponentBase{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    /// <typeparam name="TComponent">Concrete component type used to fetch the value.</typeparam>
    [Serializable]
    [JungleClassInfo("Value From Component", "Reads a value provided by a ValueComponent reference.", null, "Values/Core")]
    public class ValueFromComponent<T, TComponent> : ISettableValue<T>
        where TComponent : ValueComponentBase<T>
    {
        [SerializeField]
        private TComponent component;

        /// <inheritdoc />
        public T Value()
        {
            if (component == null)
            {
                Debug.LogError(
                    $"Ref reference is missing for {GetType().Name}. Assign a {typeof(TComponent).Name} instance.");
                return default;
            }

            return component.Value();
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public void SetValue(T value)
        {
            if (component == null)
            {
                Debug.LogError(
                    $"Ref reference is missing for {GetType().Name}. Assign a {typeof(TComponent).Name} instance.");
                return;
            }

            component.SetValue(value);
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public bool HasMultipleValues
        {
            get
            {
                if (component == null)
                {
                    Debug.LogError(
                        $"Ref reference is missing for {GetType().Name}. Assign a {typeof(TComponent).Name} instance.");
                    return false;
                }

                return component.HasMultipleValues;
            }
        }
    }
}
