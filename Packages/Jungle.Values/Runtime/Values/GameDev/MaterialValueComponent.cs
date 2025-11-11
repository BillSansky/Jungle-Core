using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a material reference.
    /// </summary>
    [JungleClassInfo("Material Value Component", "Component exposing a material reference.", null, "Values/Game Dev")]
    public class MaterialValueComponent : ValueComponent<Material>
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
    /// Component exposing a list of materials.
    /// </summary>

    [JungleClassInfo("Material List Component", "Component exposing a list of materials.", null, "Values/Game Dev")]
    public class MaterialListValueComponent : SerializedValueListComponent<Material>
    {
    }
    /// <summary>
    /// Reads a material reference from a MaterialValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Material Value From Component", "Reads a material reference from a MaterialValueComponent.", null, "Values/Game Dev")]
    public class MaterialValueFromComponent :
        ValueFromComponent<Material, MaterialValueComponent>, ISettableMaterialValue
    {
    }
    /// <summary>
    /// Reads materials from a MaterialListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Material List From Component", "Reads materials from a MaterialListValueComponent.", null, "Values/Game Dev")]
    public class MaterialListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Material>, MaterialListValueComponent>
    {
    }
}
