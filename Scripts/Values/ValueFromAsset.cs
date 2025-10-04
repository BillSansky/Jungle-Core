using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueAsset{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the asset.</typeparam>
    /// <typeparam name="TAsset">Concrete asset type used to fetch the value.</typeparam>
    [Serializable]
    public class ValueFromAsset<T, TAsset> : IValue<T>
        where TAsset : ValueAsset<T>
    {
        [SerializeField]
        private TAsset valueAsset;

        /// <inheritdoc />
        public T Value()
        {
            if (valueAsset == null)
            {
                Debug.LogError(
                    $"value asset is not assigned for {GetType().Name}. Please assign a {typeof(TAsset).Name} instance.");
                return default;
            }

            return valueAsset.Value();
        }

        public bool HasMultipleValues => valueAsset.HasMultipleValues;
    }
}
