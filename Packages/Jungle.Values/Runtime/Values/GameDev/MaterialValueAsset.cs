using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a material reference.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material value", fileName = "MaterialValue")]
    [JungleClassInfo("Material Value Asset", "ScriptableObject storing a material reference.", null, "Values/Game Dev")]
    public class MaterialValueAsset : ValueAsset<Material>
    {
        [SerializeField]
        private Material value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Material Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Material value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of materials.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material list value", fileName = "MaterialListValue")]
    [JungleClassInfo("Material List Asset", "ScriptableObject storing a list of materials.", null, "Values/Game Dev")]
    public class MaterialListValueAsset : SerializedValueListAsset<Material>
    {
    }
    /// <summary>
    /// Reads a material reference from a MaterialValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Material Value From Asset", "Reads a material reference from a MaterialValueAsset.", null, "Values/Game Dev")]
    public class MaterialValueFromAsset :
        ValueFromAsset<Material, MaterialValueAsset>, ISettableMaterialValue
    {
    }
    /// <summary>
    /// Reads materials from a MaterialListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Material List From Asset", "Reads materials from a MaterialListValueAsset.", null, "Values/Game Dev")]
    public class MaterialListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Material>, MaterialListValueAsset>
    {
    }
}
