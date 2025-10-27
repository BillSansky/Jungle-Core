using System;
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
    }

    [Serializable]
    public class ComponentValueFromComponent : ValueFromComponent<Component, ComponentValueComponent>, IComponentValue
    {
    }
}
