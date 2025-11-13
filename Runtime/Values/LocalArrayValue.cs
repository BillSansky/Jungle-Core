using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Exposes items from a serialized array on the owner.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Local Array Value", "Exposes items from a serialized array on the owner.", null, "Values/Core", true)]
    public class LocalArrayValue<T> : IValueSableValue<T>
    {
        [SerializeField]
        private T[] values = Array.Empty<T>();
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>
        public T Value()
        {
            return values.Length > 0 ? values[0] : default;
        }
        public void SetValue(T value)
        {
            if (values == null || values.Length == 0)
            {
                values = new[] { value };
                return;
            }

            values[0] = value;
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public bool HasMultipleValues => values.Length > 1;
        /// <summary>
        /// Enumerates all available values from the provider.
        /// </summary>

        public IEnumerable<T> Values
        {
            get
            {
                foreach (var value in values)
                {
                    yield return value;
                }
            }
        }
    }
}