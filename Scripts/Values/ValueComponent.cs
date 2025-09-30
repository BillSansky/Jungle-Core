using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Base class for ScriptableObject assets that expose a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the asset.</typeparam>
    public abstract class ValueComponent<T> : MonoBehaviour, IValue<T>
    {
        /// <inheritdoc />
        public abstract T GetValue();
    }
}