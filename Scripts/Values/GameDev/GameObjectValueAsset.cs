using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a GameObject reference.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject value", fileName = "GameObjectValue")]
    [JungleClassInfo("GameObject Value Asset", "ScriptableObject storing a GameObject reference.", null, "Values/Game Dev")]
    public class GameObjectValueAsset : ValueAsset<GameObject>
    {
        [SerializeField]
        private GameObject value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override GameObject Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(GameObject value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of GameObjects.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/GameObject list value", fileName = "GameObjectListValue")]
    [JungleClassInfo("GameObject List Asset", "ScriptableObject storing a list of GameObjects.", null, "Values/Game Dev")]
    public class GameObjectListValueAsset : SerializedValueListAsset<GameObject>
    {
    }
    /// <summary>
    /// Reads a GameObject reference from a GameObjectValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject Value From Asset", "Reads a GameObject reference from a GameObjectValueAsset.", null, "Values/Game Dev")]
    public class GameObjectValueFromAsset :
        ValueFromAsset<GameObject, GameObjectValueAsset>, IGameObjectValue
    {
    }
    /// <summary>
    /// Reads GameObjects from a GameObjectListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject List From Asset", "Reads GameObjects from a GameObjectListValueAsset.", null, "Values/Game Dev")]
    public class GameObjectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<GameObject>, GameObjectListValueAsset>
    {
    }
}
