using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides GameObject references from a serialized array.
    /// </summary>
    [Serializable]
    [JungleClassInfo("GameObject Array Value", "Provides GameObject references from a serialized array.", null, "Game Dev", true)]
    public class GameObjectLocalArrayValue : LocalArrayValue<GameObject>, IGameObjectValue
    {
    }
}