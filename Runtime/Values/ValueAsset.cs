using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Base class for ScriptableObject assets that expose a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the asset.</typeparam>
    [JungleClassInfo("Value Asset Base", "Base ScriptableObject for exposing runtime values.", null, "Core")]
    public abstract class ValueAssetBase<T> : ScriptableObject, ISettableValue<T>
    {
        /// <inheritdoc />
        public abstract T Value();
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public abstract void SetValue(T value);
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public abstract bool HasMultipleValues { get; }
    }

    /// <summary>
    /// Asset base class for values represented by a single element.
    /// </summary>
    /// <typeparam name="T">Type of the value provided by the asset.</typeparam>
    [JungleClassInfo("Value Asset", "ScriptableObject storing a single value instance.", null, "Core")]
    public abstract class ValueAsset<T> : ValueAssetBase<T>
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
    }

    /// <summary>
    /// Asset base class for values represented by a list of elements.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements contained in the list.</typeparam>
    [JungleClassInfo("Value List Asset", "ScriptableObject storing a list of values.", null, "Core")]
    public abstract class ValueListAsset<TElement> : ValueAssetBase<IReadOnlyList<TElement>>
    {
        /// <summary>
        /// Provides access to the list of values stored in the asset.
        /// </summary>
        protected abstract IReadOnlyList<TElement> Items { get; }

        /// <summary>
        /// Replaces the list stored in the asset.
        /// </summary>
        /// <param name="value">New list reference.</param>
        protected abstract void SetItems(IReadOnlyList<TElement> value);
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override IReadOnlyList<TElement> Value()
        {
            return Items;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(IReadOnlyList<TElement> value)
        {
            SetItems(value);
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => Items.Count > 1;

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Accesses an element at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the element to access.</param>
        public TElement this[int index] => Items[index];
    }

    /// <summary>
    /// Asset base class for list values stored as a serialized <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements contained in the list.</typeparam>
    [JungleClassInfo("Serialized Value List Asset", "Value list asset backed by a serialized List.", null, "Core")]
    public abstract class SerializedValueListAsset<TElement> : ValueListAsset<TElement>
    {
        [SerializeField]
        private List<TElement> values = new();

        protected override IReadOnlyList<TElement> Items => values;

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
