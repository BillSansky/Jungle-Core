using System;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// Abstract base class for creating conditions that can be evaluated in the Octoputs system.
    /// Inherit from this class to create custom conditions that return true/false based on your logic.
    /// </summary>
    [Serializable]
    public abstract class Condition
    {
        [SerializeField]
        private bool negateCondition;

        /// <summary>
        /// Evaluates whether the condition is currently valid.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise.</returns>
        public bool IsValid()
        {
            return negateCondition ? !IsValidImpl() : IsValidImpl();
        }
        
        protected internal abstract bool IsValidImpl();
    }
}