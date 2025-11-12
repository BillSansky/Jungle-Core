using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a GameObject reference.
    /// </summary>
    [JungleClassInfo("GameObject Value Component", "Component exposing a GameObject reference.", null, "Values/Game Dev")]
    public class GameObjectValueComponent : ValueComponent<GameObject>
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
    /// Component exposing a list of GameObjects.
    /// </summary>

    [JungleClassInfo("GameObject List Component", "Component exposing a list of GameObjects.", null, "Values/Game Dev")]
    public class GameObjectListValueComponent : SerializedValueListComponent<GameObject>
    {
    }
    /// <summary>
    /// Reads a GameObject reference from a GameObjectValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject Value From Component", "Reads a GameObject reference from a GameObjectValueComponent.", null, "Values/Game Dev")]
    public class GameObjectValueFromComponent :
        ValueFromComponent<GameObject, GameObjectValueComponent>, ISettableGameObjectValue
    {
    }
    /// <summary>
    /// Reads GameObjects from a GameObjectListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject List From Component", "Reads GameObjects from a GameObjectListValueComponent.", null, "Values/Game Dev")]
    public class GameObjectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<GameObject>, GameObjectListValueComponent>
    {
    }
}
