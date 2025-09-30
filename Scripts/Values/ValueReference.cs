using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Serializable reference that can provide a value by delegating to a selectable <see cref="ValueSource{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided through the reference.</typeparam>
    [Serializable]
    public class ValueReference<T> : IValue<T>
    {
        [SerializeReference]
        private ValueSource<T> valueSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReference{T}"/> class.
        /// </summary>
        public ValueReference()
        {
            EnsureValueSource();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReference{T}"/> class with a predefined local value.
        /// </summary>
        /// <param name="value">Initial value assigned to a local value source.</param>
        public ValueReference(T value)
        {
            valueSource = new LocalValue<T>(value);
        }

        /// <inheritdoc />
        public T GetValue()
        {
            EnsureValueSource();
            return valueSource.GetValue();
        }

        /// <summary>
        /// Replaces the current value source with a different implementation.
        /// </summary>
        /// <param name="newSource">New value source to use. If null, a <see cref="LocalValue{T}"/> will be created.</param>
        public void SetValueSource(ValueSource<T> newSource)
        {
            valueSource = newSource ?? new LocalValue<T>();
        }

        private void EnsureValueSource()
        {
            if (valueSource == null)
            {
                valueSource = new LocalValue<T>();
            }
        }
    }
}
