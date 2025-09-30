using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject Value", fileName = "GameObjectValue")]
    public class GameObjectValueAsset : ValueAsset<GameObject>
    {
        [SerializeField]
        private GameObject value;

        public override GameObject GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class GameObjectValueFromAsset :
        ValueFromAsset<GameObject, GameObjectValueAsset>, IGameObjectValue
    {
    }
}
