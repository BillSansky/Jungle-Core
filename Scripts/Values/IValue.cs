using System.Collections;
using System.Collections.Generic;

namespace Jungle.Values
{
    /// <summary>
    /// Provides access to a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value returned by the provider.</typeparam>
    public interface IValue<out T>
    {
        /// <summary>
        /// Gets the value represented by the provider.
        /// </summary>
        /// <returns>The value produced by the provider.</returns>
        T Value();

        T V => Value();

        public bool HasMultipleValues { get; }

        IEnumerable<T> Values
        {
            get { yield return Value(); }
        }
    }
    /// <summary>
    /// Defines the ISettableValue contract.
    /// </summary>
    public interface ISettableValue<T> : IValue<T>
    {
        /// <summary>
        /// Replaces the value produced by the provider.
        /// </summary>
        void SetValue(T value);
    }
}
