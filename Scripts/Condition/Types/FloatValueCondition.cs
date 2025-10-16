﻿using System;
using Jungle.Attributes;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Conditions
{
    [Serializable]
    [JungleClassInfo("Float Value Condition", "Evaluates a float value against a comparison.", null, "Values")]
    public class FloatValueCondition : Condition
    {
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
        [JungleClassSelection(typeof(IFloatValue))]
        [SerializeReference]
        private IFloatValue valueProvider;

        [SerializeField]
        private ComparisonOperator comparisonOperator = ComparisonOperator.Equals;

        [SerializeField]
        [JungleClassSelection(typeof(IFloatValue))]
        [SerializeReference]
        private IFloatValue comparisonValue;

        [SerializeField]
        private float tolerance = 0.0001f;

        protected internal override bool IsValidImpl()
        {
            if (valueProvider == null)
            {
                throw new InvalidOperationException("Value provider reference has not been assigned on FloatValueCondition.");
            }

            if (comparisonValue == null)
            {
                throw new InvalidOperationException("Comparison value reference has not been assigned on FloatValueCondition.");
            }

            float value = valueProvider.Value();
            float comparison = comparisonValue.Value();

            return comparisonOperator switch
            {
                ComparisonOperator.Equals => Mathf.Abs(value - comparison) <= tolerance,
                ComparisonOperator.NotEquals => Mathf.Abs(value - comparison) > tolerance,
                ComparisonOperator.GreaterThan => value > comparison,
                ComparisonOperator.LessThan => value < comparison,
                ComparisonOperator.GreaterThanOrEqual => value >= comparison,
                ComparisonOperator.LessThanOrEqual => value <= comparison,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
