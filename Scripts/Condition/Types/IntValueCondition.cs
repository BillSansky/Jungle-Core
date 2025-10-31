using System;
using Jungle.Attributes;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// Evaluates an integer value against a comparison operator.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Int Value Condition", "Evaluates an int value against a comparison.", null, "Values")]
    public class IntValueCondition : Condition
    {
        /// <summary>
        /// Lists the integer comparison operations available to the condition.
        /// </summary>
        public enum ComparisonOperator
        {
            Equals,
            NotEquals,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual
        }

        [SerializeField]
        [JungleClassSelection(typeof(IIntValue))]
        [SerializeReference]
        private IIntValue valueProvider;

        [SerializeField]
        private ComparisonOperator comparisonOperator = ComparisonOperator.Equals;

        [SerializeField]
        [JungleClassSelection(typeof(IIntValue))]
        [SerializeReference]
        private IIntValue comparisonValue;
        /// <summary>
        /// Evaluates the configured comparison between the provided int values.
        /// </summary>
        protected internal override bool IsValidImpl()
        {
            if (valueProvider == null)
            {
                throw new InvalidOperationException("Value provider reference has not been assigned on IntValueCondition.");
            }

            if (comparisonValue == null)
            {
                throw new InvalidOperationException("Comparison value reference has not been assigned on IntValueCondition.");
            }

            int value = valueProvider.Value();
            int comparison = comparisonValue.Value();

            return comparisonOperator switch
            {
                ComparisonOperator.Equals => value == comparison,
                ComparisonOperator.NotEquals => value != comparison,
                ComparisonOperator.GreaterThan => value > comparison,
                ComparisonOperator.LessThan => value < comparison,
                ComparisonOperator.GreaterThanOrEqual => value >= comparison,
                ComparisonOperator.LessThanOrEqual => value <= comparison,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
