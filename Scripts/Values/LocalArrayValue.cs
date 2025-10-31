using System;
using System.Collections.Generic;

namespace Jungle.Values
{
    /// <summary>
    /// Generic local value storing multiple items directly on the host object.
    /// </summary>
    [Serializable]
    public class LocalArrayValue<T> : IValue<T>
    {
        private T[] values;
        /// <summary>
        /// Returns the first element from the locally stored array.
        /// </summary>
        public T Value()
        {
            return values[0];
        }

        public bool HasMultipleValues => values.Length > 1;

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