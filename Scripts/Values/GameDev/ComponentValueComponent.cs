using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
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

    public class ComponentListValueComponent : SerializedValueListComponent<Component>
    {
    }

    [Serializable]
    public class ComponentValueFromComponent : ValueFromComponent<Component, ComponentValueComponent>, IComponentValue
    {
    }

    [Serializable]
    public class ComponentListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Component>, ComponentListValueComponent>
    {
    }
}
