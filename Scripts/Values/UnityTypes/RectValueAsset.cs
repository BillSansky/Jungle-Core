using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Rect value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Rect value", fileName = "RectValue")]
    public class RectValueAsset : ValueAsset<Rect>
    {
        [SerializeField]
        private Rect value = new Rect(0f, 0f, 1f, 1f);
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Rect Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Rect value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Rect values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Rect list value", fileName = "RectListValue")]
    public class RectListValueAsset : SerializedValueListAsset<Rect>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Rect value from the assigned RectValueAsset.
    /// </summary>
    [Serializable]
    public class RectValueFromAsset : ValueFromAsset<Rect, RectValueAsset>, IRectValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Rect values from an assigned RectListValueAsset.
    /// </summary>
    [Serializable]
    public class RectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rect>, RectListValueAsset>
    {
    }
}
