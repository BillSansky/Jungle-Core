using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueAsset{T}"/> ScriptableObject.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the ScriptableObject.</typeparam>
    /// <typeparam name="T1"></typeparam>
    [Serializable]
    public class ValueFromScriptableObject<T,T1> : IValue<T> where T1 : ValueAsset<T>
    {
        [SerializeField]
        private T1 valueAsset;

        /// <inheritdoc />
        public T GetValue()
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
