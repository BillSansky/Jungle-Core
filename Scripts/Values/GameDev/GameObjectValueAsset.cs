using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject value", fileName = "GameObjectValue")]
    [JungleClassInfo("GameObject Value Asset", "ScriptableObject storing a GameObject reference.", null, "Values/Game Dev")]
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
    [JungleClassInfo("GameObject List Asset", "ScriptableObject storing a list of GameObjects.", null, "Values/Game Dev")]
    public class GameObjectListValueAsset : SerializedValueListAsset<GameObject>
    {
    }

    [Serializable]
    [JungleClassInfo("GameObject Value From Asset", "Reads a GameObject reference from a GameObjectValueAsset.", null, "Values/Game Dev")]
    public class GameObjectValueFromAsset :
        ValueFromAsset<GameObject, GameObjectValueAsset>, IGameObjectValue
    {
    }

    [Serializable]
    [JungleClassInfo("GameObject List From Asset", "Reads GameObjects from a GameObjectListValueAsset.", null, "Values/Game Dev")]
    public class GameObjectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<GameObject>, GameObjectListValueAsset>
    {
    }
}
