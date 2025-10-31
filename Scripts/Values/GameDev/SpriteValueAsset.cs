using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a Sprite reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite value", fileName = "SpriteValue")]
    public class SpriteValueAsset : ValueAsset<Sprite>
    {
        [SerializeField]
        private Sprite value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Sprite Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Sprite value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Sprite references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite list value", fileName = "SpriteListValue")]
    public class SpriteListValueAsset : SerializedValueListAsset<Sprite>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Sprite reference from the assigned SpriteValueAsset.
    /// </summary>
    [Serializable]
    public class SpriteValueFromAsset : ValueFromAsset<Sprite, SpriteValueAsset>, ISpriteValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Sprite references from an assigned SpriteListValueAsset.
    /// </summary>
    [Serializable]
    public class SpriteListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Sprite>, SpriteListValueAsset>
    {
    }
}
