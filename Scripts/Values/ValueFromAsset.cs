using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueAssetBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the asset.</typeparam>
    /// <typeparam name="TAsset">Concrete asset type used to fetch the value.</typeparam>
    [Serializable]
    public class ValueFromAsset<T, TAsset> : ISettableValue<T>
        where TAsset : ValueAssetBase<T>
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
        /// <summary>
        /// Forwards the value update to the referenced asset.
        /// </summary>
        public void SetValue(T value)
        {
            if (valueAsset == null)
            {
                Debug.LogError(
                    $"value asset is not assigned for {GetType().Name}. Please assign a {typeof(TAsset).Name} instance.");
                return;
            }

            valueAsset.SetValue(value);
        }

        public bool HasMultipleValues
        {
            get
            {
                if (valueAsset == null)
                {
                    Debug.LogError(
                        $"value asset is not assigned for {GetType().Name}. Please assign a {typeof(TAsset).Name} instance.");
                    return false;
                }

                return valueAsset.HasMultipleValues;
            }
        }
    }
}
