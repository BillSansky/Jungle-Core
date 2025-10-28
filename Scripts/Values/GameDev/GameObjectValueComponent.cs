using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
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

    public class GameObjectListValueComponent : SerializedValueListComponent<GameObject>
    {
    }

    [Serializable]
    public class GameObjectValueFromComponent :
        ValueFromComponent<GameObject, GameObjectValueComponent>, IGameObjectValue
    {
    }

    [Serializable]
    public class GameObjectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<GameObject>, GameObjectListValueComponent>
    {
    }
}
