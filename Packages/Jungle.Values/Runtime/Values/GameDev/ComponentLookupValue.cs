using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves a Component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Component Lookup Value", "Retrieves a Component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Game Dev")]
    public class ComponentLookupValue : BaseComponentLookupValue<Component, IComponentValue>, IComponentValue
    {
    }
}
