using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves a Collider component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collider Lookup Value", "Retrieves a Collider component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Game Dev")]
    public class ColliderLookupValue : BaseComponentLookupValue<Collider, IColliderValue>, IColliderValue
    {
    }
}
