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
    [JungleClassInfo("a value only defined locally",true)]
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
        /// Returns the value currently stored on this reference.
        /// </summary>
        public T Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the stored value.
        /// </summary>
        public virtual void SetValue(T value)
        {
            this.value = value;
        }

        public virtual bool HasMultipleValues => false;
    }
}
