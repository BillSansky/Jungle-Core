using System;
using Jungle.Attributes;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Conditions
{
    [Serializable]
    [JungleClassInfo("Bool Value Condition", "Evaluates a bool value against an expected value.", null, "Values")]
    public class BoolValueCondition : Condition
    {
        [SerializeField]
        [JungleClassSelection(typeof(IBoolValue))]
        [SerializeReference]
        private IBoolValue valueProvider;

        [SerializeField]
        [SerializeReference]
        [JungleClassSelection]
        private IBoolValue expectedValue = new BoolValue(true);

        protected internal override bool IsValidImpl()
        {
            if (valueProvider == null)
            {
                throw new InvalidOperationException("Value provider reference has not been assigned on BoolValueCondition.");
            }

            return valueProvider.Value() == expectedValue.Value();
        }
    }
}
