using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a Component reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Ref value", fileName = "ComponentValueAsset")]
    public class ComponentValueAsset : ValueAsset<Component>
    {
        [SerializeField]
        private Component value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Component Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Component value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Component references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Ref list value", fileName = "ComponentListValueAsset")]
    public class ComponentListValueAsset : SerializedValueListAsset<Component>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Component reference from the assigned ComponentValueAsset.
    /// </summary>
    [Serializable]
    public class ComponentValueFromAsset : ValueFromAsset<Component, ComponentValueAsset>, IComponentValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Component references from an assigned ComponentListValueAsset.
    /// </summary>
    [Serializable]
    public class ComponentListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Component>, ComponentListValueAsset>
    {
    }
}
