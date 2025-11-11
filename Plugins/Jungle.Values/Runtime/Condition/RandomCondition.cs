using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// A condition that returns true based on a configurable probability.
    /// Useful for triggering random events or adding variability to behaviours.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Random Condition", "Evaluates to true based on a probability threshold", null, "General")]
    public class RandomCondition : Condition
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float probability = 0.5f;

        /// <summary>
        /// Evaluates the random condition.
        /// </summary>
        /// <returns>True when the generated random value is below the configured probability.</returns>
        protected internal override bool IsValidImpl()
        {
            return UnityEngine.Random.value < probability;
        }
    }
}
