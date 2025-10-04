using System;
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
    }

    [Serializable]
    public class GameObjectValueFromComponent :
        ValueFromComponent<GameObject, GameObjectValueComponent>, IGameObjectValue
    {
    }
}
