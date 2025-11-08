using System;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// Abstract base class for creating conditions that can be evaluated in the Octoputs system.
    /// Inherit from this class to create custom conditions that return true/false based on your logic.
    /// </summary>
    [Serializable]
    public abstract class Condition : IBoolValue
    {
        [SerializeField]
        private bool invertCondition;

        /// <summary>
        /// Evaluates whether the condition is currently valid.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise.</returns>
        public bool IsValid()
        {
            return invertCondition ? !IsValidImpl() : IsValidImpl();
        }
        
        protected internal abstract bool IsValidImpl();
        /// <summary>
        /// Evaluates the condition and returns the result.
        /// </summary>
        public bool Value()
        {
            return IsValid();
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public bool HasMultipleValues => false;
    }
}