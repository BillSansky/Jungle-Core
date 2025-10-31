using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Base class for <see cref="MonoBehaviour"/> components that expose a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    public abstract class ValueComponentBase<T> : MonoBehaviour, ISettableValue<T>
    {
        /// <inheritdoc />
        public abstract T Value();
        /// <summary>
        /// Assigns a new value to the component's underlying data source.
        /// </summary>
        public abstract void SetValue(T value);

        public abstract bool HasMultipleValues { get; }
    }

    /// <summary>
    /// Ref base class for values represented by a single element.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the component.</typeparam>
    public abstract class ValueComponent<T> : ValueComponentBase<T>
    {
        public override bool HasMultipleValues => false;
    }

    /// <summary>
    /// Ref base class for values represented by a list of elements.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements contained in the list.</typeparam>
    public abstract class ValueListComponent<TElement> : ValueComponentBase<IReadOnlyList<TElement>>
    {
        /// <summary>
        /// Provides access to the list of values exposed by the component.
        /// </summary>
        protected abstract IReadOnlyList<TElement> Items { get; }

        /// <summary>
        /// Replaces the list used by the component.
        /// </summary>
        /// <param name="value">New list reference.</param>
        protected abstract void SetItems(IReadOnlyList<TElement> value);
        /// <summary>
        /// Returns the list currently exposed by the component.
        /// </summary>
        public override IReadOnlyList<TElement> Value()
        {
            return Items;
        }
        /// <summary>
        /// Replaces the component's list with the provided collection.
        /// </summary>
        public override void SetValue(IReadOnlyList<TElement> value)
        {
            SetItems(value);
        }

        public override bool HasMultipleValues => Items.Count > 1;

        /// <summary>
        /// Gets the number of elements exposed by the component.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Accesses an element at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the element to access.</param>
        public TElement this[int index] => Items[index];
    }

    /// <summary>
    /// Ref base class for list values stored as a serialized <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements contained in the list.</typeparam>
    public abstract class SerializedValueListComponent<TElement> : ValueListComponent<TElement>
    {
        [SerializeField]
        private List<TElement> values = new();

        protected override IReadOnlyList<TElement> Items => values;
        /// <summary>
        /// Copies the supplied collection into the serialized backing list.
        /// </summary>
        protected override void SetItems(IReadOnlyList<TElement> value)
        {
            if (value is List<TElement> list)
            {
                values = list;
                return;
            }

            values = value == null ? new List<TElement>() : new List<TElement>(value);
        }
    }
}
