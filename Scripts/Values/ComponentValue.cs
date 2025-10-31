using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Stores a Component reference directly on the owning object for reuse.
    /// </summary>
    [Serializable]
    public class ComponentLocalValue : LocalValue<Component>, IComponent<Component>
    {
        public override bool HasMultipleValues => false;
    }
    /// <summary>
    /// Resolves a Component reference from a selected member on another component.
    /// </summary>
    [Serializable]
    public class ComponentClassMembers : ClassMembersValue<Component>,IComponent<Component>
    {
    }
}
