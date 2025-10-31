using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Component Value Component", "Component exposing a component reference.", null, "Values/Game Dev")]
    public class ComponentValueComponent : ValueComponent<Component>
    {
        [SerializeField]
        private Component value;

        public override Component Value()
        {
            return value;
        }

        public override void SetValue(Component value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Component List Component", "Component exposing a list of component references.", null, "Values/Game Dev")]
    public class ComponentListValueComponent : SerializedValueListComponent<Component>
    {
    }

    [Serializable]
    [JungleClassInfo("Component Value From Component", "Reads a component reference from a ComponentValueComponent.", null, "Values/Game Dev")]
    public class ComponentValueFromComponent : ValueFromComponent<Component, ComponentValueComponent>, IComponentValue
    {
    }

    [Serializable]
    [JungleClassInfo("Component List From Component", "Reads component references from a ComponentListValueComponent.", null, "Values/Game Dev")]
    public class ComponentListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Component>, ComponentListValueComponent>
    {
    }
}
