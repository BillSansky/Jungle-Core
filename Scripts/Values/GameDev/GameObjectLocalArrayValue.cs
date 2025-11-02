using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [Serializable]
    [JungleClassInfo("GameObject Array Value", "Provides GameObject references from a serialized array.", null, "Values/Game Dev", true)]
    public class GameObjectLocalArrayValue : LocalArrayValue<GameObject>, IGameObjectValue
    {
    }
}