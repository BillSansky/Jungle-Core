using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value sourced from a <see cref="ValueAssetBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the asset.</typeparam>
    /// <typeparam name="TAsset">Concrete asset type used to fetch the value.</typeparam>
    [Serializable]
    [JungleClassInfo("Value From Asset", "Reads a value provided by a ScriptableObject asset.", null, "Values/Core")]
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
        /// Assigns a new value to the provider.
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
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

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
