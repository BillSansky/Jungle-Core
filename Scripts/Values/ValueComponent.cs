using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Base class for <see cref="MonoBehaviour"/> components that expose a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    public abstract class ValueComponent<T> : MonoBehaviour, IValue<T>
    {
        /// <inheritdoc />
        public abstract T Value();
        
        public bool HasMultipleValues => false;
        
        
    }
}