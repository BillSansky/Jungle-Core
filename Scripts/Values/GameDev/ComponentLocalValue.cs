using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to a component reference.
    /// </summary>
    public interface IComponentValue : IComponent<Component>
    {
    }
    /// <summary>
    /// Stores a component reference directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Component Value", "Stores a component reference directly on the owner.", null, "Values/Game Dev", true)]
    public class ComponentLocalValue : LocalValue<Component>, IComponentValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
    }
}
