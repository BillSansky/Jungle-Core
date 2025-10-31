using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a Material reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material value", fileName = "MaterialValue")]
    public class MaterialValueAsset : ValueAsset<Material>
    {
        [SerializeField]
        private Material value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Material Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Material value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Material references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material list value", fileName = "MaterialListValue")]
    public class MaterialListValueAsset : SerializedValueListAsset<Material>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Material reference from an assigned MaterialValueAsset.
    /// </summary>
    [Serializable]
    public class MaterialValueFromAsset :
        ValueFromAsset<Material, MaterialValueAsset>, IMaterialValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Material references from an assigned MaterialListValueAsset.
    /// </summary>
    [Serializable]
    public class MaterialListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Material>, MaterialListValueAsset>
    {
    }
}
