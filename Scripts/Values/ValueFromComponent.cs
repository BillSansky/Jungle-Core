using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueComponentBase{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    /// <typeparam name="TComponent">Concrete component type used to fetch the value.</typeparam>
    [Serializable]
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
