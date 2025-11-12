using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a component reference.
    /// </summary>
    [JungleClassInfo("Component Value Component", "Component exposing a component reference.", null, "Game Dev")]
    public class ComponentValueComponent : ValueComponent<Component>
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
    /// Component exposing a list of component references.
    /// </summary>

    [JungleClassInfo("Component List Component", "Component exposing a list of component references.", null, "Game Dev")]
    public class ComponentListValueComponent : SerializedValueListComponent<Component>
    {
    }
    /// <summary>
    /// Reads a component reference from a ComponentValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Component Value From Component", "Reads a component reference from a ComponentValueComponent.", null, "Game Dev")]
    public class ComponentValueFromComponent : ValueFromComponent<Component, ComponentValueComponent>, ISettableComponentValue
    {
    }
    /// <summary>
    /// Reads component references from a ComponentListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Component List From Component", "Reads component references from a ComponentListValueComponent.", null, "Game Dev")]
    public class ComponentListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Component>, ComponentListValueComponent>
    {
    }
}
