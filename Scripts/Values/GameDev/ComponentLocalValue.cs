using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IComponentValue : IComponent<Component>
    {
    }

    [Serializable]
    [JungleClassInfo("Component Value", "Stores a component reference directly on the owner.", null, "Values/Game Dev", true)]
    public class ComponentLocalValue : LocalValue<Component>, IComponentValue
    {
        public override bool HasMultipleValues => false;
    }
}
