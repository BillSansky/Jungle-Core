using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jungle.Conditions
{
    /// <summary>
    /// A simple boolean condition that can be toggled in the inspector.
    /// Useful for testing or creating simple on/off conditions.
    /// </summary>
    [Serializable]
    public class ExternalCondition : Condition
    {
        [SerializeField] private Object conditionProvider;
        
        /// <summary>
        /// Evaluates the boolean condition.
        /// </summary>
        /// <returns>The current value of the boolean condition.</returns>
        protected internal override bool IsValidImpl()
        {
            return ((IBooleanCondition)conditionProvider).Condition;
        }
    }
    
    public interface IBooleanCondition
    {
        bool Condition { get; }
    }

}