using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// Condition that combines multiple other conditions with configurable logic.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Combinatory Condition","Combines a few conditions together with logical operators",null,"General")]
    public class CombinatoryCondition : Condition
    {
        [JungleClassSelection] [SerializeReference]
        private List<Condition> conditions;
        /// <summary>
        /// Combines all child conditions using the selected logical operator.
        /// </summary>
        [SerializeField] private LogicalOperator logicalOperator = LogicalOperator.And;

        protected internal override bool IsValidImpl()
        {
            if (conditions == null || conditions.Count == 0)
                return false;

            return logicalOperator switch
            {
                LogicalOperator.And => EvaluateAnd(),
                LogicalOperator.Or => EvaluateOr(),
                LogicalOperator.Xor => EvaluateXor(),
                LogicalOperator.Nand => !EvaluateAnd(),
                LogicalOperator.Nor => !EvaluateOr(),
                _ => false
            };
        }
        /// <summary>
        /// Returns true only when every child condition evaluates to true.
        /// </summary>
        private bool EvaluateAnd()
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsValid())
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Returns true when at least one child condition evaluates to true.
        /// </summary>
        private bool EvaluateOr()
        {
            foreach (var condition in conditions)
            {
                if (condition.IsValid())
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Returns true when exactly one child condition evaluates to true.
        /// </summary>
        private bool EvaluateXor()
        {
            int trueCount = 0;
            foreach (var condition in conditions)
            {
                if (condition.IsValid())
                {
                    trueCount++;
                    if (trueCount > 1)
                        return false;
                }
            }

            return trueCount == 1;
        }
    }
}