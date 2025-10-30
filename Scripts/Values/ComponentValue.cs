using System;
using UnityEngine;

namespace Jungle.Values
{
    [Serializable]
    public class ComponentLocalValue : LocalValue<Component>, IComponent<Component>
    {
        public override bool HasMultipleValues => false;
    }

    [Serializable]
    public class ComponentClassMembers : ClassMembersValue<Component>,IComponent<Component>
    {
    }
}
