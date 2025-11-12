using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Retrieves a Rigidbody component from a GameObject using a lookup strategy.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Rigidbody Lookup Value", "Retrieves a Rigidbody component from a GameObject using a lookup strategy (on object, in children, or in parent).", null, "Game Dev")]
    public class RigidbodyLookupValue : BaseComponentLookupValue<Rigidbody, IRigidbodyValue>, IRigidbodyValue
    {
    }
}
