using System;
using Jungle.Attributes;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// Evaluates a double value against a comparison.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Double Value Condition", "Evaluates a double value against a comparison.", null, "Comparisons")]
    public class DoubleValueCondition : Condition
    {
        /// <summary>
        /// Defines the comparison operators supported by the condition.
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
        [JungleClassSelection(typeof(IDoubleValue))]
        [SerializeReference]
        private IDoubleValue valueProvider;

        [SerializeField]
        private ComparisonOperator comparisonOperator = ComparisonOperator.Equals;

        [SerializeField]
        [JungleClassSelection(typeof(IDoubleValue))]
        [SerializeReference]
        private IDoubleValue comparisonValue;

        [SerializeField]
        private double tolerance = 0.0001;

        protected internal override bool IsValidImpl()
        {
            if (valueProvider == null)
            {
                throw new InvalidOperationException("Value provider reference has not been assigned on DoubleValueCondition.");
            }

            if (comparisonValue == null)
            {
                throw new InvalidOperationException("Comparison value reference has not been assigned on DoubleValueCondition.");
            }

            double value = valueProvider.Value();
            double comparison = comparisonValue.Value();

            return comparisonOperator switch
            {
                ComparisonOperator.Equals => Math.Abs(value - comparison) <= tolerance,
                ComparisonOperator.NotEquals => Math.Abs(value - comparison) > tolerance,
                ComparisonOperator.GreaterThan => value > comparison,
                ComparisonOperator.LessThan => value < comparison,
                ComparisonOperator.GreaterThanOrEqual => value >= comparison,
                ComparisonOperator.LessThanOrEqual => value <= comparison,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
