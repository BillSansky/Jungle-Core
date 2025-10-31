using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a GameObject reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject value", fileName = "GameObjectValue")]
    public class GameObjectValueAsset : ValueAsset<GameObject>
    {
        [SerializeField]
        private GameObject value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override GameObject Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(GameObject value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of GameObject references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject list value", fileName = "GameObjectListValue")]
    public class GameObjectListValueAsset : SerializedValueListAsset<GameObject>
    {
    }
    /// <summary>
    /// Value wrapper that reads a GameObject reference from an assigned GameObjectValueAsset.
    /// </summary>
    [Serializable]
    public class GameObjectValueFromAsset :
        ValueFromAsset<GameObject, GameObjectValueAsset>, IGameObjectValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of GameObject references from an assigned GameObjectListValueAsset.
    /// </summary>
    [Serializable]
    public class GameObjectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<GameObject>, GameObjectListValueAsset>
    {
    }
}
