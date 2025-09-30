using System;

namespace Jungle.Values
{
    /// <summary>
    /// Base class for value providers that can be selected at runtime via managed references.
    /// </summary>
    /// <typeparam name="T">Type of value produced by the provider.</typeparam>
    [Serializable]
    public abstract class ValueSource<T> : IValue<T>
    {
        /// <inheritdoc />
        public abstract T GetValue();
    }
}
