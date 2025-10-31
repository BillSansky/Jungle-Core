using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IComponentValue contract.
    /// </summary>
    public interface IComponentValue : IComponent<Component>
    {
    }
    /// <summary>
    /// Stores a Component reference directly on the owning object for reuse.
    /// </summary>
    [Serializable]
    public class ComponentLocalValue : LocalValue<Component>, IComponentValue
    {
        public override bool HasMultipleValues => false;
    }
}
