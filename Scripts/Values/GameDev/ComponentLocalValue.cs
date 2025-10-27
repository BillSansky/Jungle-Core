using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IComponentValue : IComponent<Component>
    {
    }

    [Serializable]
    public class ComponentLocalValue : LocalValue<Component>, IComponentValue
    {
        public override bool HasMultipleValues => false;
    }
}
