using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("GameObject Value Component", "Component exposing a GameObject reference.", null, "Values/Game Dev")]
    public class GameObjectValueComponent : ValueComponent<GameObject>
    {
        [SerializeField]
        private GameObject value;

        public override GameObject Value()
        {
            return value;
        }

        public override void SetValue(GameObject value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("GameObject List Component", "Component exposing a list of GameObjects.", null, "Values/Game Dev")]
    public class GameObjectListValueComponent : SerializedValueListComponent<GameObject>
    {
    }

    [Serializable]
    [JungleClassInfo("GameObject Value From Component", "Reads a GameObject reference from a GameObjectValueComponent.", null, "Values/Game Dev")]
    public class GameObjectValueFromComponent :
        ValueFromComponent<GameObject, GameObjectValueComponent>, IGameObjectValue
    {
    }

    [Serializable]
    [JungleClassInfo("GameObject List From Component", "Reads GameObjects from a GameObjectListValueComponent.", null, "Values/Game Dev")]
    public class GameObjectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<GameObject>, GameObjectListValueComponent>
    {
    }
}
