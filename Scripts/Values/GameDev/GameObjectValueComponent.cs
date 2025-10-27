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

        public override void SetValue(GameObject value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class GameObjectValueFromComponent :
        ValueFromComponent<GameObject, GameObjectValueComponent>, IGameObjectValue
    {
    }
}
