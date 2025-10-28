using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject value", fileName = "GameObjectValue")]
    public class GameObjectValueAsset : ValueAsset<GameObject>
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject list value", fileName = "GameObjectListValue")]
    public class GameObjectListValueAsset : SerializedValueListAsset<GameObject>
    {
    }

    [Serializable]
    public class GameObjectValueFromAsset :
        ValueFromAsset<GameObject, GameObjectValueAsset>, IGameObjectValue
    {
    }

    [Serializable]
    public class GameObjectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<GameObject>, GameObjectListValueAsset>
    {
    }
}
