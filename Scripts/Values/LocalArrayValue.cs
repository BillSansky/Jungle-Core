using System;
using System.Collections.Generic;

namespace Jungle.Values
{
    [Serializable]
    public class LocalArrayValue<T> : IValue<T>
    {
        private T[] values;
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