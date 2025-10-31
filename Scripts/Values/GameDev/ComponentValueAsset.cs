using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Ref value", fileName = "ComponentValueAsset")]
    [JungleClassInfo("Component Value Asset", "ScriptableObject storing a component reference.", null, "Values/Game Dev")]
    public class ComponentValueAsset : ValueAsset<Component>
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Ref list value", fileName = "ComponentListValueAsset")]
    [JungleClassInfo("Component List Asset", "ScriptableObject storing a list of component references.", null, "Values/Game Dev")]
    public class ComponentListValueAsset : SerializedValueListAsset<Component>
    {
    }

    [Serializable]
    [JungleClassInfo("Component Value From Asset", "Reads a component reference from a ComponentValueAsset.", null, "Values/Game Dev")]
    public class ComponentValueFromAsset : ValueFromAsset<Component, ComponentValueAsset>, IComponentValue
    {
    }

    [Serializable]
    [JungleClassInfo("Component List From Asset", "Reads component references from a ComponentListValueAsset.", null, "Values/Game Dev")]
    public class ComponentListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Component>, ComponentListValueAsset>
    {
    }
}
