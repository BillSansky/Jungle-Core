using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    [Serializable]
    [JungleClassInfo("Component Local Value", "Stores a component reference directly on the owner.", null, "Values/Game Dev", true)]
    public class ComponentLocalValue : LocalValue<Component>, IComponent<Component>
    {
        public override bool HasMultipleValues => false;
    }

    [Serializable]
    [JungleClassInfo("Component Member Value", "Fetches a component reference via a reflected member.", null, "Values/Game Dev")]
    public class ComponentClassMembers : ComponentClassMembersValue<Component>, IComponent<Component>
    {
    }
}
