using System;
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
    }

    [Serializable]
    public class ComponentValueFromAsset : ValueFromAsset<Component, ComponentValueAsset>, IComponentValue
    {
    }
}
