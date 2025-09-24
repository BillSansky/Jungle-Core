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
    public class CombinatoryCondition : Condition
    {
        [JungleList("Conditions", "Add Condition")]
        [SerializeReference] private List<Condition> conditions;
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

        private bool EvaluateAnd()
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsValid())
                    return false;
            }

            return true;
        }

        private bool EvaluateOr()
        {
            foreach (var condition in conditions)
            {
                if (condition.IsValid())
                    return true;
            }

            return false;
        }

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