using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueAsset{T}"/> ScriptableObject.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the ScriptableObject.</typeparam>
    [Serializable]
    public class ScriptableObjectValue<T> : ValueSource<T>
    {
        [SerializeField]
        private ValueAsset<T> valueAsset;

        /// <inheritdoc />
        public override T GetValue()
        {
            if (valueAsset == null)
            {
                Debug.LogError(
                    $"ScriptableObject value asset is not assigned for {GetType().Name}. Please assign a {typeof(ValueAsset<T>).Name} instance.");
                return default;
            }

            return valueAsset.GetValue();
        }
    }
}
