using System;
using System.Collections.Generic;
using Jungle.Attributes;

namespace Jungle.Values
{
    [Serializable]
    [JungleClassInfo("Local Array Value", "Exposes items from a serialized array on the owner.", null, "Values/Core", true)]
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