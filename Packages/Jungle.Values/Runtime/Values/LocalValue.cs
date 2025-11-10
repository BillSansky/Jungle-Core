using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides a value stored locally on the object using the value reference.
    /// </summary>
    /// <typeparam name="T">Type of value being stored.</typeparam>
    [Serializable]
    [JungleClassInfo("Local Value", "Stores a value directly on the referencing object.", null, "Values/Core", true)]
    public abstract class LocalValue<T> : ISettableValue<T>
    {
        /// <summary>
        /// Local value stored directly on the reference.
        /// </summary>
        [SerializeField]
        protected T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalValue{T}"/> class.
        /// </summary>
        public LocalValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalValue{T}"/> class with a starting value.
        /// </summary>
        /// <param name="value">Initial value.</param>
        public LocalValue(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>
        
        public T Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public virtual void SetValue(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public virtual bool HasMultipleValues => false;
    }
}
