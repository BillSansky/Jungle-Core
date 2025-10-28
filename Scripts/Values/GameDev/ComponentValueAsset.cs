using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Component value", fileName = "ComponentValueAsset")]
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Component list value", fileName = "ComponentListValueAsset")]
    public class ComponentListValueAsset : SerializedValueListAsset<Component>
    {
    }

    [Serializable]
    public class ComponentValueFromAsset : ValueFromAsset<Component, ComponentValueAsset>, IComponentValue
    {
    }

    [Serializable]
    public class ComponentListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Component>, ComponentListValueAsset>
    {
    }
}
