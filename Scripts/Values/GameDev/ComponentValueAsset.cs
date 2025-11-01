using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a component reference.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Ref value", fileName = "ComponentValueAsset")]
    [JungleClassInfo("Component Value Asset", "ScriptableObject storing a component reference.", null, "Values/Game Dev")]
    public class ComponentValueAsset : ValueAsset<Component>
    {
        [SerializeField]
        private Component value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Component Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Component value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of component references.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Ref list value", fileName = "ComponentListValueAsset")]
    [JungleClassInfo("Component List Asset", "ScriptableObject storing a list of component references.", null, "Values/Game Dev")]
    public class ComponentListValueAsset : SerializedValueListAsset<Component>
    {
    }
    /// <summary>
    /// Reads a component reference from a ComponentValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Component Value From Asset", "Reads a component reference from a ComponentValueAsset.", null, "Values/Game Dev")]
    public class ComponentValueFromAsset : ValueFromAsset<Component, ComponentValueAsset>, IComponentValue
    {
    }
    /// <summary>
    /// Reads component references from a ComponentListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Component List From Asset", "Reads component references from a ComponentListValueAsset.", null, "Values/Game Dev")]
    public class ComponentListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Component>, ComponentListValueAsset>
    {
    }
}
