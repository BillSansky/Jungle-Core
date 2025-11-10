using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Stores a component reference directly on the owner.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Component Local Value", "Stores a component reference directly on the owner.", null, "Values/Game Dev", true)]
    public class ComponentLocalValue : LocalValue<Component>, IComponent<Component>
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
    }
    /// <summary>
    /// Fetches a component reference via a reflected member.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Component Member Value", "Fetches a component reference via a reflected member.", null, "Values/Game Dev")]
    public class ComponentClassMembers : ComponentClassMembersValue<Component>, IComponent<Component>
    {
    }
}
