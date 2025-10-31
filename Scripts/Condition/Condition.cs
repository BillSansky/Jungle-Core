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
        /// <summary>
        /// Performs the concrete condition evaluation before inversion is applied.
        /// </summary>
        protected internal abstract bool IsValidImpl();
        /// <summary>
        /// Exposes the condition result through the <see cref="IBoolValue"/> interface.
        /// </summary>
        public bool Value()
        {
            return IsValid();
        }

        public bool HasMultipleValues => false;
    }
}